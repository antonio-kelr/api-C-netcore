using Microsoft.AspNetCore.Mvc;
using Classificados.Services;
using ClasificadoImagens.Models;
using Classificado.Interfaces;

namespace CoberturasImagens.Controllers
{
    [Route("classificadoImagem")]
    [ApiController]
    public class ClassificadoController : ControllerBase
    {
        private readonly IClassificadoImagen _classificadoImagensRepository;
        private readonly ClassificadosServices _imagemService;

        public ClassificadoController(IClassificadoImagen classificadoImagensRepository, ClassificadosServices imagemService)
        {
            _classificadoImagensRepository = classificadoImagensRepository;
            _imagemService = imagemService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassificadoImagemModel>>> GetAll()
        {
            var classificado = await _classificadoImagensRepository.GetAllAsync();

            return Ok(classificado);
        }

        // GET: api/Agenda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassificadoImagemModel>> GetById(int id)
        {
            var classificaGet = await _classificadoImagensRepository.GetByIdAsync(id);

            if (classificaGet == null)
            {
                return NotFound();
            }

            return Ok(classificaGet);
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ClassificadoImagemRequest classificadoImagemRequest, [FromForm] IEnumerable<IFormFile> images)
        {
            Console.WriteLine($"CoberturaId recebido: {classificadoImagemRequest.ClassificadoId}");

            if (classificadoImagemRequest == null || images == null)
            {
                return BadRequest("Dados inválidos.");
            }

            // Usando o serviço para fazer o upload das imagens para o Firebase
            var uploadResults = await _imagemService.UploadImagesAsync(images);

            // Criar uma lista de objetos ClassificadoImagemModel com as URLs dos arquivos enviados
            var classificadoImagens = new List<ClassificadoImagemModel>();
            foreach (var result in uploadResults)
            {
                var coberturaImagem = new ClassificadoImagemModel
                {
                    Titulo = result.FileName,
                    Url = result.Url,
                    ClassificadoId = classificadoImagemRequest.ClassificadoId
                };

                classificadoImagens.Add(coberturaImagem);
            }

            // Adicionar as imagens ao repositório
            foreach (var coberturaImagem in classificadoImagens)
            {
                _classificadoImagensRepository.CreateAsync(coberturaImagem);
            }


            // Salvar as mudanças no banco de dados
            if (await _classificadoImagensRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetById), new { id = classificadoImagens.First().Id }, classificadoImagens);
            }

            return BadRequest("Erro ao salvar as imagens.");
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var classificado = await _classificadoImagensRepository.GetByIdAsync(id);
            if (classificado == null)
            {
                return NotFound(new { message = "classificado não encontrada." });
            }

            _classificadoImagensRepository.DeleteAsync(id);
            bool saved = await _classificadoImagensRepository.SaveAllAsync();

            if (saved)
            {
                return Ok(new { message = "classificado deletada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao deletar a classificado." });
        }


    }
}
