using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto.Ping;
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

        [HttpPost]
        public IActionResult PingPost(TestRequest? testRequest)
        {
            if(testRequest == null)
            {
                return BadRequest("Không tìm thấy request");
            }
            return Ok(testRequest);
        }

        [HttpPost("authen")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "USER")]
        public IActionResult PingPostWithAuthen(TestRequest? testRequest)
        {
            if (testRequest == null)
            {
                return BadRequest("Không tìm thấy request");
            }
            return Ok(testRequest);
        }
    }
}
