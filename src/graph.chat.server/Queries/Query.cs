using System.Security.Claims;
using graph.chat.server.Responses;
using HotChocolate.AspNetCore.Authorization;

namespace graph.chat.server.Queries;

public partial class Query
{
    [Authorize]
    public InfoResponse Info(ClaimsPrincipal claimsPrincipal)
    {
        var claims = claimsPrincipal.Claims.ToList();
        var nameIdentifier = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        var username = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

        return new InfoResponse
        {
            UserId = nameIdentifier != null ? Guid.Parse(nameIdentifier) : Guid.Empty,
            Role = role ?? string.Empty,
            Username = username ?? string.Empty
        };
    }
}