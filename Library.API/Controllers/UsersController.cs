using Library.Application.Interfaces;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Library.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IRepository<User> _userRepository;

    public UsersController(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        if (string.IsNullOrEmpty(user.Email) || !user.Email.Contains("@"))
            return BadRequest("El correo electrónico no es válido.");

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? role)
    {
        var users = await _userRepository.GetAllAsync();

       
        if (!string.IsNullOrEmpty(role))
        {
            users = users.Where(u => u.Role.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(users);
    }
}