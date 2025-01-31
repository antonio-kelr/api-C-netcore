namespace Agenda.Models
{
    public record PersonRequest(string Nome, string Descricao, string? Url, string? Slug, DateTime Data);

    public class AgendaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Url { get; set; }
        public string? Slug { get; set; }
        public DateTime Data { get; set; } = DateTime.UtcNow;
        public string Descricao { get; set; }
    }
}
