namespace graph.chat.server.Responses;

public class AuthenticationResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}