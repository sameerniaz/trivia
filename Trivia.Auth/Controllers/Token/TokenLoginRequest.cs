using System.ComponentModel.DataAnnotations;

namespace Trivia.Auth.Controllers.Token;

public class TokenLoginRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}