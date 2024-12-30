using Microsoft.AspNetCore.Mvc;
using Cadastro.Services;
using Cadastro.Models;

namespace Cadastro.Controllers
{
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var token = _authService.Authenticate(loginRequest.Email, loginRequest.Senha);

            if (token == null)
                return Unauthorized("E-mail ou senha inválidos.");

            return Ok(new { Token = token });
        }

        
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
