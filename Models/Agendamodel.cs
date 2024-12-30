namespace Agenda.Models
{
    public record PersonRequest(string Nome, string Descricao, string? Url, string? Slug, DateTime Data);

    public class AgendaModel
    {
        public int Id { get; set; }  // ID do agendamento
        public string Nome { get; set; }  // Nome do agendamento
        public string? Url { get; set; }  // URL adicional do agendamento
        public string? Slug { get; set; }  // Slug para URL amigável
        public DateTime Data { get; set; } = DateTime.UtcNow;  // Data do agendamento
        public string Descricao { get; set; }  // Descrição do agendamento
    }
}
