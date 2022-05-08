using graph.chat.server.abstractions;
using graph.chat.server.Responses;

namespace graph.chat.server.Queries;

public partial class Query
{
    public async Task<AuthenticationResponse> Authenticate([Service]IAuthenticationService authenticationService, string username, string password)
    {
        var token = await authenticationService.AuthenticateAsync(username, password);
        return new AuthenticationResponse
        {
            Token = token.Value,
            Expiration = token.Expiration
        };
    }
    
    public async Task<AuthenticationResponse> Register([Service]IAuthenticationService authenticationService, string username, string password)
    {
        var token = await authenticationService.RegisterAsync(username, password);
        return new AuthenticationResponse
        {
            Token = token.Value,
            Expiration = token.Expiration
        };
    }
}