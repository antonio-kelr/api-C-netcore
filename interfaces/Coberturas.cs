using Coberturas.Models;

namespace Coberturas.intefaces
{
    public interface IACoberturas
    {
        void Create(CoberturaModel agenda);
        void UpdateCobertura(int id, CoberturaModel agenda);
        void Delete(int id);
        Task<IEnumerable<CoberturaModel>> Getall();
        Task<CoberturaModel> GetById(int id);
        Task<bool> SaveAllAsync();
    }
}