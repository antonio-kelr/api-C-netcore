using Microsoft.AspNetCore.Mvc;
using Agenda.Models;
using Agenda.Services;
using Projeto.Utilities;
using Recado.intefaces;
using Recado.Models;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecadoController : ControllerBase
    {
        private readonly IRecado _recadoRepository;

        public RecadoController(IRecado recadoRepository)
        {
            _recadoRepository = recadoRepository;
        }

        // GET: api/Agenda
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecadoModel>>> GetAll()
        {
            var recados = await _recadoRepository.Getall();

            return Ok(recados);
        }

        // GET: api/Agenda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecadoModel>> GetById(int id)
        {
            var recados = await _recadoRepository.GetById(id);

            if (recados == null)
            {
                return NotFound();
            }

            return Ok(recados);
        }

        // POST: api/Agenda
        [HttpPost]
        public async Task<ActionResult<RecadoModel>> Create(RecadoModel recado)
        {

            _recadoRepository.Create(recado);
            bool saved = await _recadoRepository.SaveAllAsync();

            if (saved)
            {
                return CreatedAtAction(nameof(GetById), new { id = recado.Id }, recado);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RecadoModel updatedRecado)
        {


            var existingAgenda = await _recadoRepository.GetById(id);

            if (existingAgenda == null)
            {
                return NotFound(new { message = "recado não encontrada." });
            }


            updatedRecado.Id = existingAgenda.Id;

            _recadoRepository.UpdateRecado(id, updatedRecado);

            if (await _recadoRepository.SaveAllAsync())
            {
                return Ok(new { message = "recado atualizada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao atualizar a recado." });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recado = await _recadoRepository.GetById(id);
            if (recado == null)
            {
                return NotFound(new { message = "recado não encontrada." });
            }

            _recadoRepository.Delete(id);
            bool saved = await _recadoRepository.SaveAllAsync();

            if (saved)
            {
                return Ok(new { message = "recado deletada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao deletar a recado." });
        }
    }
}
