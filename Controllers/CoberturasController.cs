using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coberturas.Models;
using Coberturas.intefaces;
using Coberturas.Repositories;
using Projeto.Utilities;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoberturaController : ControllerBase
    {
        private readonly IACoberturas _coberturaRepositories;

        public CoberturaController(IACoberturas coberturaRepositories)
        {
            _coberturaRepositories = coberturaRepositories;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoberturaModel>>> GetAll()
        {
            var coberturas = await _coberturaRepositories.Getall();
            return Ok(coberturas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CoberturaModel>> GetById(int id)
        {
            var coberturas = await _coberturaRepositories.GetById(id);

            if (coberturas == null)
            {
                return NotFound();
            }

            return Ok(coberturas);
        }

        [HttpPost]
        public async Task<ActionResult<CoberturaModel>> Create(CoberturaModel cobertura)
        {
            if (string.IsNullOrEmpty(cobertura.Titulo))
            {
                return BadRequest(new { message = "O campo 'Titulo' é obrigatório." });
            }

            cobertura.Slug = SlugGenerator.GenerateSlug(cobertura.Titulo);
            _coberturaRepositories.Create(cobertura);
            bool saved = await _coberturaRepositories.SaveAllAsync();

            if (saved)
            {
                return CreatedAtAction(nameof(GetById), new { id = cobertura.Id }, cobertura);
            }


            return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CoberturaModel updatedCobertura)
        {
            if (string.IsNullOrEmpty(updatedCobertura.Titulo))
            {
                return BadRequest(new { message = "O campo 'Titulo' é obrigatório." });
            }

            updatedCobertura.Slug = SlugGenerator.GenerateSlug(updatedCobertura.Titulo);

            var existingCobertura = await _coberturaRepositories.GetById(id);

            if (existingCobertura == null)
            {
                return NotFound(new { message = "cobertura não encontrada." });
            }
            updatedCobertura.Id = existingCobertura.Id;

            _coberturaRepositories.UpdateCobertura(id, updatedCobertura);

            if (await _coberturaRepositories.SaveAllAsync())
            {
                return Ok(new { message = "cobertura atualizada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao atualizar a cobertura." });
        }

        // DELETE: api/Agenda/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cobertura = await _coberturaRepositories.GetById(id);
            if (cobertura == null)
            {
                return NotFound(new { message = "cobertura não encontrada." });
            }

            _coberturaRepositories.Delete(id);
            bool saved = await _coberturaRepositories.SaveAllAsync();

            if (saved)
            {
                return Ok(new { message = "cobertura deletada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao deletar a cobertura." });
        }
    }
}
