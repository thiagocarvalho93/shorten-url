using System.Security.Cryptography;
using ShorterUrl.DTOs;
using ShorterUrl.Exceptions;
using ShorterUrl.Models;
using ShorterUrl.Repository;
using System.Security.Authentication;

namespace ShorterUrl.Service;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly JwtService _jwtService;

    public UserService(UserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<UserModel> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id) ?? throw new NotFoundException("User not found.");

        return user;
    }

    public async Task<UserModel> AddUserAsync(UserInsertRequestDTO userRequest)
    {
        if (await _userRepository.UserExistsAsync(userRequest.Username))
        {
            throw new InvalidOperationException("User with the same username already exists.");
        }

        var hashedPassword = HashPassword(userRequest.Password);

        var user = new UserModel
        {
            DateCreated = DateTime.Now,
            Email = userRequest.Email,
            FirstName = userRequest.FirstName,
            IsActive = true,
            LastName = userRequest.LastName,
            PasswordHash = hashedPassword,
            Username = userRequest.Username
        };

        await _userRepository.AddUserAsync(user);

        return user;
    }

    public async Task UpdateUserAsync(UserModel user)
    {
        if (!await _userRepository.UserExistsAsync(user.Username))
        {
            throw new NotFoundException("User not found.");
        }

        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id) ?? throw new NotFoundException("User not found.");

        await _userRepository.DeleteUserAsync(user.Id);
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _userRepository.UserExistsAsync(username);
    }

    public async Task<string> Login(LoginRequestDTO loginRequestDTO)
    {
        var user = await ValidateUserAsync(loginRequestDTO.Username, loginRequestDTO.Password) ?? throw new AuthenticationException("Invalid username or password.");

        return _jwtService.GenerateToken(user);
    }

    private async Task<UserModel?> ValidateUserAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (user != null && VerifyPassword(password, user.PasswordHash))
        {
            return user;
        }

        return null;
    }

    private static string HashPassword(string password)
    {
        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        return Convert.ToBase64String(hashBytes);
    }

    private static bool VerifyPassword(string enteredPassword, string storedHash)
    {
        byte[] hashBytes = Convert.FromBase64String(storedHash);

        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        using var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
        byte[] hash = pbkdf2.GetBytes(20);

        for (int i = 0; i < 20; i++)
        {
            if (hashBytes[i + 16] != hash[i])
            {
                return false;
            }
        }

        return true;
    }
}