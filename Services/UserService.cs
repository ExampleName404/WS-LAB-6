using TaskManagementApi.Models;

namespace TaskManagementApi.Services;

public class UserService
{
    private readonly List<AppUser> _users = new()
    {
        new AppUser { Username = "student", Password = "12345", Role = "User" },
        new AppUser { Username = "teacher", Password = "qwerty", Role = "Admin" }
    };

    public AppUser? Validate(string username, string password)
    {
        return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}
