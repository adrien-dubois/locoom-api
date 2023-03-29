using Microsoft.AspNetCore.Mvc;

namespace Locoom.API.Controllers
{
    [Route("[controller]")]
    public class DinnersController : ApiController
    {
        [HttpGet]
        public IActionResult ListDinners()
        {
            return Ok(Array.Empty<string>());
        }
    }
}
