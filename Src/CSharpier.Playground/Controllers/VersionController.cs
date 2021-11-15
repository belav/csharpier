using Microsoft.AspNetCore.Mvc;

namespace CSharpier.Playground.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return typeof(CodeFormatter).Assembly.GetName().Version?.ToString(3);
    }
}
