using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using server.Constant;
using server.Dto.Auth;
using server.Entity;
using server.Utils;

namespace server.Controllers
{

    [ApiController]
    [Route("/api/v1/auth")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;


        public AuthController(IUserRepository userRepository, IRoleRepository roleRepository, IRepository<UserRole> userRoleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
        }

        [HttpPost("/login")]
        public IActionResult Login(LoginRequest request)
        {
            User? user = _userRepository.FindByEmail(request.Email);
            if (user == null)
            {
                throw new ApplicationException("Không tìm thấy thông tin user");
            }

            if(!user.Password.Equals(request.Password))
            {
                throw new ApplicationException("Sai mật khẩu");
            }

            AccessTokenResponse accessTokenResponse = JwtUtils.GenerateAccessToken(user);
            return Ok(accessTokenResponse);
        }

        [HttpPost("/refresh")]
        public IActionResult RefresToken(RefreshTokenRequest request)
        {
          
            RefreshTokenResponse refreshTokenResponse = JwtUtils.Refreshtoken(request.RefreshToken);

            return Ok(refreshTokenResponse);
        }

        [HttpPost("/signup")]
        public IActionResult Register(RegisterRequest request)
        {
            User? user = _userRepository.FindByEmail(request.Email);
            if (user != null)
            {
                throw new ApplicationException("Email đã tồn tại");
            }

            user = new User
            {
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _userRepository.Add(user);

            Role? role = _roleRepository.findByName(RoleEnum.USER);

            if (role != null) {
                UserRole userRole = new UserRole
                {
                    UserId = user.UserId,
                    RoleId = role.RoleId
                };

                _userRoleRepository.Add(userRole);
            }

            return Ok(user);
        }
    }
}
