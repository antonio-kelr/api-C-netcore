using Microsoft.AspNetCore.Mvc;
using banner.Services;
using Projeto.Utilities;
using Banner.intefaces;
using banner.Models;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BannerController : ControllerBase
    {
        private readonly IBanner _bannerRepositories;
        private readonly FirebaseImageBanner _imageService;

        public BannerController(IBanner bannerRepositories, FirebaseImageBanner imageService)
        {
            _bannerRepositories = bannerRepositories;
            _imageService = imageService;
        }

        // GET: api/recado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BannerModel>>> GetAll()
        {
            var recados = await _bannerRepositories.Getall();

            return Ok(recados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BannerModel>> GetById(int id)
        {
            var recado = await _bannerRepositories.GetById(id);

            if (recado == null)
            {
                return NotFound();
            }

            return Ok(recado);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PersonRequest bannerRequest, [FromForm] IFormFile image)
        {
            Console.WriteLine($"UserId recebido: {bannerRequest.UserId}");

            if (bannerRequest == null || image == null || image.Length == 0)
            {
                return BadRequest("Dados inválidos ou imagem não fornecida.");
            }

            // Fazer upload da imagem para o Firebase
            var imageUrl = await _imageService.UploadImageAsync(image);
            string titulo = Path.GetFileNameWithoutExtension(image.FileName);

            // Criar o objeto BannerModel com o nome do arquivo como título
            var banner = new BannerModel
            {
                Titulo = titulo, // Nome do arquivo sem extensão
                Url = imageUrl,
                UserId = bannerRequest.UserId ?? 0
            };

            // Adicionar ao repositório
            _bannerRepositories.Create(banner);

            // Salvar no banco de dados
            if (await _bannerRepositories.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetById), new { id = banner.Id }, banner);
            }

            return BadRequest("Erro ao salvar a imagem.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recado = await _bannerRepositories.GetById(id);
            if (recado == null)
            {
                return NotFound(new { message = "recado não encontrada." });
            }

            _bannerRepositories.Delete(id);
            bool saved = await _bannerRepositories.SaveAllAsync();

            if (saved)
            {
                return Ok(new { message = "recado deletada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao deletar a recado." });
        }
    }
}
