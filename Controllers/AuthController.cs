using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.Models;
using TaskManagementApi.Services;

namespace TaskManagementApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _users;
    private readonly TokenService _tokens;

    public AuthController(UserService users, TokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var user = _users.Validate(request.Username, request.Password);
        if (user == null)
            return Unauthorized(new { message = "Неверный логин или пароль" });

        var (token, expires) = _tokens.CreateToken(user);
        return Ok(new { token, expires, username = user.Username, role = user.Role });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var claims = HttpContext.User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }
}
