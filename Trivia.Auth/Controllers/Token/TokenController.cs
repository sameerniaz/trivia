using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Trivia.Auth.Services;

namespace Trivia.Auth.Controllers.Token;

[ApiController]
[Route("token")]
public class TokenController : ControllerBase
{
    private readonly UserService _userService;

    public TokenController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<TokenLoginResponse>> LoginAsync([FromBody] TokenLoginRequest loginInfo)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.LoginUser(loginInfo.Username, loginInfo.Password);

            var response = new TokenLoginResponse
            {
                AccessToken = "access_token_here"
            };
            return Ok(response);
        }
        catch (InvalidCredentialsException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("revoke")]
    public async Task<IActionResult> LogoutAsync()
    {
        var accessToken = Request.Headers[HeaderNames.Authorization];
        if (accessToken.Count == 0)
            return Unauthorized();

        // Remove access token from active sessions

        return NoContent();
    }
}