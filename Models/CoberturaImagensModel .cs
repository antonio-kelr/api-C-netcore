using Coberturas.Models;

namespace CoberturasImagens.Models
{
    public record CoberturaImagemRequest(string? Titulo, string? Url, int CoberturaId);

    public class CoberturaImagemModel
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Url { get; set; }
        public int CoberturaId { get; set; } // Chave estrangeira
        public CoberturaModel? Cobertura { get; set; } // Propriedade de navegação (pode ser nullable)

          public override string ToString()
    {
        return $"Id: {Id}, Titulo: {Titulo}, Url: {Url}, CoberturaId: {CoberturaId}, Cobertura: {Cobertura?.ToString() ?? "null"}";
    }
    }


}
