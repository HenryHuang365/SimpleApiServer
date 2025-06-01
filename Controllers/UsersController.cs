using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel;

namespace SimpleApiServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IMemoryCache _cache;

    public UsersController(IUserService service, IMemoryCache cache)
    {
        _service = service;
        _cache = cache;
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(UserProfileDto[]), StatusCodes.Status201Created)]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>
    /// Gets a user by id.
    /// </summary>
    /// <param name="id">The id of the user to find.</param>
    /// <returns>User details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> GetUserById(
    [DefaultValue("6b6502d5-a2de-4f2e-ab8d-0c74c6ad8dff")] Guid id)
    {
        try
        {
            var user = await _service.GetByIdAsync(id);
            return Ok(user);
        }
        catch (NotFoundException exception)
        {
            return NotFound(new ErrorResponseDto { Message = exception.Message });
        }
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    [HttpPost]
    [SwaggerRequestExample(typeof(CreateUserDto), typeof(CreateUserDtoExample))]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> createUser([FromBody] CreateUserDto user)
    {
        var newUser = await _service.CreateAsync(user);
        return Ok(newUser);
    }
}
