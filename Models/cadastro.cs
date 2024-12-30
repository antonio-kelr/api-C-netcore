using System.ComponentModel.DataAnnotations;


namespace Cadastro.Models
{
    public record CadastroRequest(string Nome, string Email, string Senha);

    public class CadastroModel
    {
        public int Id { get; set; }
        [MinLength(3)]  
        public string Nome { get; set; }

        [EmailAddress] 
        [MinLength(6)]  
        public string Email { get; set; }

        [MinLength(6)]  
        public string Senha { get; set; }
    }
}
