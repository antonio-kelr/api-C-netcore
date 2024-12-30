namespace Noticias.Models
{
    public record NoticiasRequest(string Titulo, string Descricao, string? Slug, string? Url, DateTime Data);

    public class NoticiasModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string? Slug { get; set; }
        public string? Url { get; set; }
        public DateTime Data { get; set; } = DateTime.UtcNow;
        public string Descricao { get; set; }
    }
}
