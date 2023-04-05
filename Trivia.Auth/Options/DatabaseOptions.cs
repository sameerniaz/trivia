namespace Trivia.Auth.Options;

public class DatabaseOptions
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string UsersCollectionName { get; set; }
}