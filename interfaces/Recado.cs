using Recado.Models;

namespace Recado.intefaces
{
    public interface IRecado
    {
        void Create(RecadoModel recado);
        void UpdateRecado(int id, RecadoModel recado);
        void Delete(int id);
        Task<IEnumerable<RecadoModel>> Getall();
        Task<RecadoModel> GetById(int id);
        Task<bool> SaveAllAsync();
    }
}