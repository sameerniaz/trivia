using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Trivia.Auth.Services;

namespace Trivia.Auth.Controllers.User;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> CreateUserAsync(CreateUserRequest newUserInfo)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.CreateUserAsync(newUserInfo.Username, newUserInfo.Password);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Models.User, CreateUserResponse>());
            var mapper = config.CreateMapper();
            var response = mapper.Map<CreateUserResponse>(user);

            return Created("", response);
        }
        catch (UserAlreadyExistsException)
        {
            ModelState.AddModelError("Username", "A user with the provided username already exists.");
            return Conflict(ModelState);
        }
    }
}