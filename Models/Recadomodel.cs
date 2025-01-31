namespace Recado.Models
{
    public record PersonRequest(string Remetente, string Mensagem, string Destinatario, DateTime Data, string Slug);

    public class RecadoModel
    {
        public int Id { get; set; }
        public required string Remetente { get; set; }
        public required string Destinatario { get; set; }
        public string? Slug { get; set; }

        public DateTime Data { get; set; } = DateTime.UtcNow;
        public required string Mensagem { get; set; }
    }
}
