using System.ComponentModel.DataAnnotations;

namespace Trivia.Auth.Controllers.User;

public class CreateUserRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}