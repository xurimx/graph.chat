using Microsoft.AspNetCore.Mvc;

namespace graph.chat.server.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : Controller
{
    // GET
    public string Get()
    {
        return "hello test";
    }
}