using CoberturasImagens.Models;
using System.Text.Json.Serialization;

namespace Coberturas.Models
{
    public record CoberturaRequest(string Titulo, string Descricao, string? Slug, DateTime Data, string Local, string Fotografo);

    public class CoberturaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Local { get; set; }
        public string Fotografo { get; set; }
        public string? Slug { get; set; }
        public DateTime Data { get; set; } = DateTime.UtcNow;  // Data do agendamento
        public string Descricao { get; set; }
        public ICollection<CoberturaImagemModel> Imagens { get; set; } = new List<CoberturaImagemModel>();

    }


}
