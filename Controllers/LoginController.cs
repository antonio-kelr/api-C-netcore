using Microsoft.AspNetCore.Mvc;
using Cadastro.Services;

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
            // Chama o serviço de autenticação
            var authResult = _authService.Authenticate(loginRequest.Email, loginRequest.Senha);

            // Verifica se o resultado é nulo (usuário não encontrado ou senha incorreta)
            if (authResult == null)
                return Unauthorized("E-mail ou senha inválidos.");

            // Retorna tanto o token quanto o UserId na resposta
            return Ok(new { Token = authResult.Token, UserId = authResult.UserId });
        }
    }



    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}
