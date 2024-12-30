using Cadastro.Models;

namespace Cadastro.intefaces
{
    public interface IACadastro
    {
        Task Create(CadastroModel cadastro);
        Task<IEnumerable<CadastroModel>> Getall();
    }
}