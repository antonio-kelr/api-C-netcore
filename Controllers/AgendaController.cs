using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agenda.Models;
using Agenda.intefaces;
using Agenda.Repositories;
using Agenda.Services;
using Projeto.Utilities;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendaController : ControllerBase
    {
        private readonly IAgenda _agendaRepository;
        private readonly FirebaseImageService _imageService;

        public AgendaController(IAgenda agendaRepository, FirebaseImageService imageService)
        {
            _agendaRepository = agendaRepository;
            _imageService = imageService;
        }

        // GET: api/Agenda
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgendaModel>>> GetAll()
        {
            var agendas = await _agendaRepository.Getall();

            return Ok(agendas);
        }

        // GET: api/Agenda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgendaModel>> GetById(int id)
        {
            var agenda = await _agendaRepository.GetById(id);

            if (agenda == null)
            {
                return NotFound();
            }

            return Ok(agenda);
        }

        // POST: api/Agenda
        [HttpPost]
        public async Task<ActionResult<AgendaModel>> Create([FromForm] AgendaModel agenda, IFormFile image)
        {
            // Verifica se a imagem foi enviada
            if (image == null || image.Length == 0)
            {
                return BadRequest(new { message = "A imagem não foi fornecida." });
            }

            agenda.Slug = SlugGenerator.GenerateSlug(agenda.Nome);


            // Faz o upload da imagem e recebe o URL da imagem no Firebase
            var imageService = new FirebaseImageService();
            var imageUrl = await imageService.UploadImageAsync(image);

            // Atribui o URL da imagem à agenda
            agenda.Url = imageUrl;

            _agendaRepository.Create(agenda);
            bool saved = await _agendaRepository.SaveAllAsync();

            if (saved)
            {
                return CreatedAtAction(nameof(GetById), new { id = agenda.Id }, agenda);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AgendaModel updatedAgenda)
        {
            var existingAgenda = await _agendaRepository.GetById(id);

            if (existingAgenda == null)
            {
                return NotFound(new { message = "Agenda não encontrada." });
            }

            _agendaRepository.UpdateAgenda(id, updatedAgenda);

            if (await _agendaRepository.SaveAllAsync())
            {
                return Ok(new { message = "Agenda atualizada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao atualizar a agenda." });
        }

        // DELETE: api/Agenda/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var agenda = await _agendaRepository.GetById(id);
            if (agenda == null)
            {
                return NotFound(new { message = "Agenda não encontrada." });
            }

            _agendaRepository.Delete(id);
            bool saved = await _agendaRepository.SaveAllAsync();

            if (saved)
            {
                return Ok(new { message = "Agenda deletada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao deletar a agenda." });
        }
    }
}
