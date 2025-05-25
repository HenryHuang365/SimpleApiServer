using Microsoft.AspNetCore.Mvc;

namespace SimpleApiServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
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
