using Microsoft.EntityFrameworkCore;
using ShorterUrl.Data;
using ShorterUrl.Models;

namespace ShorterUrl.Repository;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<UserModel?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<UserModel?> GetUserByUsernameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username == userName);
    }

    public async Task<UserModel> AddUserAsync(UserModel user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task UpdateUserAsync(UserModel user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> UserExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
}