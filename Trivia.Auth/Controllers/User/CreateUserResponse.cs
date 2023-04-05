using System;

namespace Trivia.Auth.Controllers.User;

public class CreateUserResponse
{
    public string Id { get; set; }
    public string Username { get; set; }
    public DateTime CreatedTime { get; set; }
}