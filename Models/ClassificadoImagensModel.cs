using Classificados.Models;

namespace ClasificadoImagens.Models
{
    public record ClassificadoImagemRequest(string? Titulo, string? Url, int ClassificadoId);

    public class ClassificadoImagemModel
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Url { get; set; }
        public int ClassificadoId { get; set; }
        public ClassificadosModel? Classificado { get; set; }

    }


}
