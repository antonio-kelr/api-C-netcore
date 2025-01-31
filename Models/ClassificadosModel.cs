using ClasificadoImagens.Models;
namespace Classificados.Models
{
    public record ClassificadoRequest(
        string Titulo,
        string Descricao,
        string? Slug,
        DateTime Data,
        string Telefone,
        string Estado,
        string Cidade,
        string Email,
         int Categoria

        );

    public class ClassificadosModel
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public string? Slug { get; set; }
        public required decimal Preco { get; set; }
        public required string Telefone { get; set; }
        public required string Email { get; set; }
        public required string Cidade { get; set; }
        public required string Estado { get; set; }
        public int Categoria { get; set; } = 0;
        public DateTime Data { get; set; } = DateTime.UtcNow;  // Data do agendamento
        public required string Descricao { get; set; }
        public ICollection<ClassificadoImagemModel> Imagens { get; set; } = new List<ClassificadoImagemModel>();

    }


}
