using AutoMapper;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dto.User;

namespace server.Controllers.Admin
{
    [ApiController]
    [Route("/api/admin/users")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "ADMIN")]
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult List()
        {
            return Ok(_mapper.Map<List<UserResponse>>(userRepository.All()));
        }
    }
}
