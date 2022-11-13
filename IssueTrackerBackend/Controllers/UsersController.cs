using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using IssueTrackerBackend.Data;
using IssueTrackerBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IssueTrackerBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DataContext _context;
    private IConfiguration _config;

    public UsersController(DataContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(UserRegisterRequest request)
    {
        if (_context.Users.Any(u => u.Username == request.Username))
        {
            return BadRequest("Username already taken");
        }

        if (_context.Users.Any(u => u.Email == request.Email))
        {
            return BadRequest($"User with email {request.Email} already registered");
        }

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Created("", "User successfully created");
    }


    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLoginRequest request)
    {
        User user = null;
        if (IsValidEmail(request.UsernameOrEmail))
        {
            user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.UsernameOrEmail);
            if (user == null)
            {
                return BadRequest("Incorrect username or password");
            }
        }
        else
        {
            user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail);
            if (user == null)
            {
                return BadRequest("Incorrect username or password");
            }
        }

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Incorrect username or password");

        var token = GenerateToken(user);

        var result = new
        {
            token = token
        };
        return Ok(result);
    }

    private string GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        int tokenExpiresInMinutes = 15;
        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(tokenExpiresInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
    
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedPasswordHash.SequenceEqual(passwordHash);
        }
    }

}