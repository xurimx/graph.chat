using graph.chat.server.abstractions.Contracts;

namespace graph.chat.server.abstractions;

public interface IAuthenticationService
{
    Task<Token> AuthenticateAsync(string username, string password);
    Task<Token> RegisterAsync(string username, string password);
}