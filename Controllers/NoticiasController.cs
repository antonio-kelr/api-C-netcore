using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Noticias.Models;
using Noticias.intefaces;
using Noticias.Repositories;
using Noticias.Services;
using Projeto.Utilities;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoticiasController : ControllerBase
    {
        private readonly IANoticias _NoticiasRepositories;
        private readonly FirebaseImageServiceNoticias _NoticiasServices;

        public NoticiasController(IANoticias NoticiasRepositories, FirebaseImageServiceNoticias _NoticiasServices)
        {
            _NoticiasRepositories = NoticiasRepositories;
            _NoticiasServices = _NoticiasServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoticiasModel>>> GetAll()
        {
            var noticia = await _NoticiasRepositories.Getall();
            return Ok(noticia);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoticiasModel>> GetById(int id)
        {
            var noticia = await _NoticiasRepositories.GetById(id);

            if (noticia == null)
            {
                return NotFound();
            }

            return Ok(noticia);
        }

        [HttpPost]
        public async Task<ActionResult<NoticiasModel>> Create([FromForm] NoticiasModel noticia, IFormFile image)
        {

            if (image == null || image.Length == 0)
            {
                return BadRequest(new { message = "A imagem não foi fornecida." });
            }

            noticia.Slug = SlugGenerator.GenerateSlug(noticia.Titulo);


            // Faz o upload da imagem e recebe o URL da imagem no Firebase
            var imageService = new FirebaseImageServiceNoticias();
            var imageUrl = await imageService.UploadImageAsync(image);

            // Atribui o URL da imagem à noticia
            noticia.Url = imageUrl;

            _NoticiasRepositories.Create(noticia);
            bool saved = await _NoticiasRepositories.SaveAllAsync();

            if (saved)
            {
                return CreatedAtAction(nameof(GetById), new { id = noticia.Id }, noticia);
            }

            return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, NoticiasModel updatednoticia)
        {
            var existingnoticia = await _NoticiasRepositories.GetById(id);

            if (existingnoticia == null)
            {
                return NotFound(new { message = "noticia não encontrada." });
            }

            _NoticiasRepositories.UpdateNoticia(id, updatednoticia);

            if (await _NoticiasRepositories.SaveAllAsync())
            {
                return Ok(new { message = "noticia atualizada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao atualizar a noticia." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var noticia = await _NoticiasRepositories.GetById(id);
            if (noticia == null)
            {
                return NotFound(new { message = "noticia não encontrada." });
            }

            _NoticiasRepositories.Delete(id);
            bool saved = await _NoticiasRepositories.SaveAllAsync();

            if (saved)
            {
                return Ok(new { message = "noticia deletada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao deletar a noticia." });
        }
    }
}
