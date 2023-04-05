using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Trivia.Auth.Models;
using Trivia.Auth.Options;
using Trivia.Auth.Security;

namespace Trivia.Auth.Services;

public class UserService
{
    private readonly IMongoCollection<User> _collection;
    private readonly string _hashSalt;

    public UserService(IOptions<DatabaseOptions> dbOptions, IOptions<SecurityOptions> secOptions)
    {
        var client = new MongoClient(dbOptions.Value.ConnectionString);
        var database = client.GetDatabase(dbOptions.Value.DatabaseName);
        _collection = database.GetCollection<User>(dbOptions.Value.UsersCollectionName);

        _hashSalt = secOptions.Value.HashSalt;
    }

    public async Task<User> LoginUser(string username, string password)
    {
        var usernameEqualityFilter = Builders<User>.Filter
            .Eq(user => user.Username, username);
        var user = await _collection.Find(usernameEqualityFilter).FirstOrDefaultAsync();
        if (user is null)
            throw new InvalidCredentialsException();

        var passwordHash = HashingUtils.HashString(password, _hashSalt);
        if (user.PasswordHash != passwordHash)
            throw new InvalidCredentialsException();

        // Remove sensitive information before returning the user object
        user.PasswordHash = null;
        return user;
    }

    public async Task<User> CreateUserAsync(string username, string password)
    {
        var usernameEqualityFilter = Builders<User>.Filter
            .Eq(user => user.Username, username);
        var existingUser = await _collection.Find(usernameEqualityFilter).FirstOrDefaultAsync();
        if (existingUser is not null)
            throw new UserAlreadyExistsException();

        var passwordHash = HashingUtils.HashString(password, _hashSalt);

        var user = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            CreatedTime = DateTime.Now.ToUniversalTime(),
            LastLoginTime = DateTime.Now.ToUniversalTime()
        };
        await _collection.InsertOneAsync(user);
        
        // Remove sensitive information before returning the user object
        user.PasswordHash = null;
        return user;
    }
}

public class UserServiceException : Exception
{
    public UserServiceException(string message) : base(message)
    {
    }
}

public class InvalidCredentialsException : UserServiceException
{
    public InvalidCredentialsException() : base("Invalid credentials.")
    {
    }
}

public class UserAlreadyExistsException : UserServiceException
{
    public UserAlreadyExistsException() : base("A user with the provided username already exists.")
    {
    }
}