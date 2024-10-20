using ShorterUrl.Exceptions;
using ShorterUrl.Models;
using ShorterUrl.Repository;

namespace ShorterUrl.Service;

public class UserService
{
    private readonly UserRepository _userRepository;

    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
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

    public async Task AddUserAsync(UserModel user)
    {
        if (await _userRepository.UserExistsAsync(user.Username))
        {
            throw new InvalidOperationException("User with the same username already exists.");
        }

        await _userRepository.AddUserAsync(user);
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
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        await _userRepository.DeleteUserAsync(id);
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _userRepository.UserExistsAsync(username);
    }
}