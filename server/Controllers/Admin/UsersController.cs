using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers.Admin
{
    [ApiController]
    [Route("/api/admin/users")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;


        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
        public IActionResult List()
        {
            return Ok(userRepository.All());
        }
    }
}
