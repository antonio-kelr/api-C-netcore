using Microsoft.AspNetCore.Mvc;
using Projeto.Utilities;
using Classificados.intefaces;
using Classificados.Models;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassificadoController : ControllerBase
    {
        private readonly IClassificados _classificadoRepositores;

        public ClassificadoController(IClassificados classificadoRepositores)
        {
            _classificadoRepositores = classificadoRepositores;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassificadosModel>>> GetAll()
        {
            var classificado = await _classificadoRepositores.Getall();
            return Ok(classificado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassificadosModel>> GetById(int id)
        {
            var classificado = await _classificadoRepositores.GetById(id);

            if (classificado == null)
            {
                return NotFound();
            }

            return Ok(classificado);
        }

        public async Task<ActionResult<ClassificadosModel>> Create(ClassificadosModel classificado)
        {
            if (string.IsNullOrEmpty(classificado.Titulo))
            {
                return BadRequest(new { message = "O campo 'Titulo' é obrigatório." });
            }

            classificado.Slug = SlugGenerator.GenerateSlug(classificado.Titulo);


            // Continuar o processo de criação
            _classificadoRepositores.Create(classificado);
            bool saved = await _classificadoRepositores.SaveAllAsync();

            if (saved)
            {
                return CreatedAtAction(nameof(GetById), new { id = classificado.Id }, classificado);
            }

            return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClassificadosModel updatedclassificado)
        {
            if (string.IsNullOrEmpty(updatedclassificado.Titulo))
            {
                return BadRequest(new { message = "O campo 'Titulo' é obrigatório." });
            }

            updatedclassificado.Slug = SlugGenerator.GenerateSlug(updatedclassificado.Titulo);

            var existingclassificado = await _classificadoRepositores.GetById(id);

            if (existingclassificado == null)
            {
                return NotFound(new { message = "classificado não encontrada." });
            }
            updatedclassificado.Id = existingclassificado.Id;

            _classificadoRepositores.Update(id, updatedclassificado);

            if (await _classificadoRepositores.SaveAllAsync())
            {
                return Ok(new { message = "classificado atualizada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao atualizar a classificado." });
        }

        // DELETE: api/Agenda/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var classificado = await _classificadoRepositores.GetById(id);
            if (classificado == null)
            {
                return NotFound(new { message = "classificado não encontrada." });
            }

            _classificadoRepositores.Delete(id);
            bool saved = await _classificadoRepositores.SaveAllAsync();

            if (saved)
            {
                return Ok(new { message = "classificado deletada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao deletar a classificado." });
        }
    }
}
