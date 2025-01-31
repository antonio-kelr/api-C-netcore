using Coberturas.Models;

namespace CoberturasImagens.Models
{
    public record CoberturaImagemRequest(string? Titulo, string? Url, int CoberturaId);

    public class CoberturaImagemModel
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Url { get; set; }
        public int CoberturaId { get; set; }
        public CoberturaModel? Cobertura { get; set; }

    }


}
