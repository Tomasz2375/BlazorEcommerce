using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlazorEcommerce.Server.Services.AuthenticationServices;

public class AuthenticationService : IAuthenticationService
{
    private readonly DataContext dataContext;
    private readonly IConfiguration configuration;
    private readonly IHttpContextAccessor httpContextAccessor;

    public AuthenticationService(
        DataContext dataContext,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        this.dataContext = dataContext;
        this.configuration = configuration;
        this.httpContextAccessor = httpContextAccessor;
    }

    public int GetUserId() =>
        int.Parse(httpContextAccessor
            .HttpContext!
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier));

    public string GetUserEmail() =>
        httpContextAccessor
            .HttpContext!
            .User
            .FindFirstValue(ClaimTypes.Name);

    public async Task<ServiceResponse<string>> Login(string email, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await dataContext.Users!
            .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
        if (user is null)
        {
            response.Sucess = false;
            response.Message = $"User with email: {email} not found.";
        }
        else if(!verifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            response.Sucess = false;
            response.Message = "Wrong password.";
        }
        else
        {
            response.Sucess = true;
            response.Data = createToken(user);
        }

        return response;
    }

    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
        if (await UserExist(user.Email))
        {
            return new ServiceResponse<int>()
            {
                Sucess = false,
                Message = "User already exists."
            };
        }

        createPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        dataContext.Users!.Add(user);
        await dataContext.SaveChangesAsync();

        return new ServiceResponse<int>()
        {
            Data = user.Id,
            Message = "Registration successful!",
            Sucess = true,
        };
    }

    public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
    {
        var user = await dataContext.Users!.FindAsync(userId);

        if (user is null)
        {
            return new ServiceResponse<bool>()
            {
                Sucess = false,
                Message = "User not found."
            };
        }

        createPasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        await dataContext.SaveChangesAsync();

        return new ServiceResponse<bool>()
        {
            Sucess = true,
            Data = true,
            Message = "Password has been changed",
        };
    }

    public async Task<bool> UserExist(string email) =>
        await dataContext
        .Users!
        .AnyAsync(x => x.Email.ToLower().Equals(email.ToLower()));


    private void createPasswordHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool verifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }

    private string createToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration.GetSection("AppSettings:Token").Value));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await dataContext
            .Users!
            .FirstOrDefaultAsync(x => x.Email.Equals(email))!;
    }
}
