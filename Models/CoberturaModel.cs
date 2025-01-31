using CoberturasImagens.Models;
using System.Text.Json.Serialization;

namespace Coberturas.Models
{
    public record CoberturaRequest(string Titulo, string Descricao, string? Slug, DateTime Data, string Local, string Fotografo);

    public class CoberturaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Local { get; set; } = string.Empty;
        public string Fotografo { get; set; } = string.Empty;
        public string? Slug { get; set; } = string.Empty;
        public DateTime Data { get; set; } = DateTime.UtcNow;
        public string Descricao { get; set; } = string.Empty;
        public ICollection<CoberturaImagemModel> Imagens { get; set; } = new List<CoberturaImagemModel>();

    }


}
