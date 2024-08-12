namespace Vault.API.Authentication;

public interface IUserRepository
{
    Task<User?> GetUser(string username, string password);
}

public class UserRepository : IUserRepository
{
    public async Task<User?> GetUser(string username, string password)
    {
        return _users.FirstOrDefault(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && x.Password.Equals(password));
    }


    private List<User> _users = new List<User>()
    {
        new User() {Username = "consumer", Password = "123", Name = "public-api", Role = "consumer"},
        new User() {Username = "author", Password = "123", Name = "author-api", Role = "author"},
    };
}
