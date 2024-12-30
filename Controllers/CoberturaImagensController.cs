using CoberturasImagens.Interfaces;
using CoberturasImagens.Models;
using CoberturaImagens.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoberturasImagens.Controllers
{
    [Route("coberturaImagem")]
    [ApiController]
    public class CoberturaImagensController : ControllerBase
    {
        private readonly ICoberturaImagens _coberturaImagensRepository;
        private readonly CoberturaImagensServices _imagemService;

        public CoberturaImagensController(ICoberturaImagens coberturaImagensRepository, CoberturaImagensServices imagemService)
        {
            _coberturaImagensRepository = coberturaImagensRepository;
            _imagemService = imagemService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoberturaImagemModel>>> GetAll()
        {
            var agendas = await  _coberturaImagensRepository.GetAllAsync();

            return Ok(agendas);
        }

        // GET: api/Agenda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoberturaImagemModel>> GetById(int id)
        {
            var coberturaImg = await  _coberturaImagensRepository.GetByIdAsync(id);

            if (coberturaImg == null)
            {
                return NotFound();
            }

            return Ok(coberturaImg);
        }



        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CoberturaImagemRequest coberturaImagemRequest, [FromForm] IEnumerable<IFormFile> images)
        {
              Console.WriteLine($"CoberturaId recebido: {coberturaImagemRequest}");
            if (coberturaImagemRequest == null || images == null)
            {
                return BadRequest("Dados inválidos.");
            }

            // Usando o serviço para fazer o upload das imagens para o Firebase
            var uploadResults = await _imagemService.UploadImagesAsync(images);

            // Criar uma lista de objetos CoberturaImagemModel com as URLs dos arquivos enviados
            var coberturaImagens = new List<CoberturaImagemModel>();
            foreach (var result in uploadResults)
            {
                var coberturaImagem = new CoberturaImagemModel
                {
                    Titulo = result.FileName,
                    Url = result.Url,  
                    CoberturaId = coberturaImagemRequest.CoberturaId
                };

                coberturaImagens.Add(coberturaImagem);
            }

            // Adicionar as imagens ao repositório
            foreach (var coberturaImagem in coberturaImagens)
            {
                _coberturaImagensRepository.CreateAsync(coberturaImagem);
            }


            // Salvar as mudanças no banco de dados
            if (await _coberturaImagensRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetById), new { id = coberturaImagens.First().Id }, coberturaImagens);
            }

            return BadRequest("Erro ao salvar as imagens.");
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cobertura = await _coberturaImagensRepository.GetByIdAsync(id);
            if (cobertura == null)
            {
                return NotFound(new { message = "cobertura não encontrada." });
            }

            _coberturaImagensRepository.DeleteAsync(id);
            bool saved = await _coberturaImagensRepository.SaveAllAsync();

            if (saved)
            {
                return Ok(new { message = "cobertura deletada com sucesso!" });
            }

            return BadRequest(new { message = "Erro ao deletar a cobertura." });
        }


    }
}
