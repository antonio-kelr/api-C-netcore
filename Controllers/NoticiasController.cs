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
        private readonly FirebaseImageServiceNoticias? _NoticiasServices;


        public NoticiasController(IANoticias NoticiasRepositories, FirebaseImageServiceNoticias NoticiasServices)
        {
            _NoticiasRepositories = NoticiasRepositories ?? throw new ArgumentNullException(nameof(NoticiasRepositories));
            _NoticiasServices = NoticiasServices ?? throw new ArgumentNullException(nameof(NoticiasServices));
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
            if (string.IsNullOrEmpty(noticia.Titulo))
            {
                return BadRequest(new { message = "O campo 'Titulo' é obrigatório." });
            }
            noticia.Slug = SlugGenerator.GenerateSlug(noticia.Titulo);


            if (image == null || image.Length == 0)
            {
                return BadRequest(new { message = "A imagem não foi fornecida." });
            }



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
        public async Task<IActionResult> Update(int id, [FromForm] NoticiasModel updatedAgenda, [FromForm] IFormFile? image)
        {
            if (string.IsNullOrEmpty(updatedAgenda.Titulo))
            {
                return BadRequest(new { message = "O campo 'Titulo' é obrigatório." });
            }
            updatedAgenda.Slug = SlugGenerator.GenerateSlug(updatedAgenda.Titulo);

            var existingAgenda = await _NoticiasRepositories.GetById(id);

            if (existingAgenda == null)
            {
                return NotFound(new { message = "Agenda não encontrada." });
            }

            // Valida se o serviço está inicializado
            if (_NoticiasServices == null)
            {
                return StatusCode(500, new { message = "Serviço de upload de imagem não está configurado." });
            }

            // Processa a imagem apenas se ela for enviada
            if (image != null && image.Length > 0)
            {
                try
                {
                    var imageUrl = await _NoticiasServices.UploadImageAsync(image);
                    updatedAgenda.Url = imageUrl; // Atualiza o campo de URL
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Erro ao fazer upload da imagem.", detalhes = ex.Message });
                }
            }
            else
            {
                updatedAgenda.Url = existingAgenda.Url; // Mantém o URL atual
            }

            // Garante que o ID do modelo atualizado corresponda ao ID original
            updatedAgenda.Id = existingAgenda.Id;

            _NoticiasRepositories.UpdateNoticia(id, updatedAgenda);

            if (await _NoticiasRepositories.SaveAllAsync())
            {
                return Ok(new { message = "Agenda atualizada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao atualizar a agenda." });
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
