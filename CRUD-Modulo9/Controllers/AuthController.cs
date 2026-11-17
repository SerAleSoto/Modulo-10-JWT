using CRUD_Modulo9.DTOs;
using CRUD_Modulo9.Services;
using Microsoft.AspNetCore.Mvc;


namespace CRUD_Modulo9.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;

        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var token = _service.Login(request.Username, request.Password);

            if (token == null)
                return Unauthorized();

            HttpContext.Session.SetString("JwtToken", token);

            return Ok(new
            {
                token
            });
        }
    }
}
