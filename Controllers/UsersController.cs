using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
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
}
