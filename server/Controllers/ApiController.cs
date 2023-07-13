using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using server.Dto.User;

namespace server.Controllers
{
    [ApiController]
    [Route("/api/v1/ping")]
    public class ApiController : Controller
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("Pong");
        }
    }
}
