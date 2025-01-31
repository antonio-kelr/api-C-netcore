using Microsoft.AspNetCore.Mvc;
using Cadastro.Models;
using Cadastro.intefaces;
using System.Threading.Tasks;

namespace Cadastro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private readonly IACadastro _cadastroRepository;

        public CadastroController(IACadastro cadastroRepository)
        {
            _cadastroRepository = cadastroRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CadastroRequest cadastroRequest)
        {
            if (cadastroRequest == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var cadastro = new CadastroModel
            {
                Nome = cadastroRequest.Nome,
                Email = cadastroRequest.Email,
                Senha = cadastroRequest.Senha
            };

            await _cadastroRepository.Create(cadastro);
            return Ok("Cadastro realizado com sucesso.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cadastros = await _cadastroRepository.Getall();
                return Ok(cadastros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao recuperar os cadastros: {ex.Message}");
            }
        }
    }
}
