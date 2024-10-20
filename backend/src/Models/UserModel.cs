namespace ShorterUrl.Models;

public class UserModel
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}