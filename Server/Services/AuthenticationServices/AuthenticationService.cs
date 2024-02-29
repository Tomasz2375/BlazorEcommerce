using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BlazorEcommerce.Server.Services.AuthenticationServices;

public class AuthenticationService : IAuthenticationService
{
    private readonly DataContext dataContext;

    public AuthenticationService(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }


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
            response.Data = "token";
        }

        response.Data = "token";

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
}
