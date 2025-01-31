using Classificados.Models;

namespace Classificados.intefaces
{
    public interface IClassificados
    {
        void Create(ClassificadosModel classificado);
        void Update(int id, ClassificadosModel classificado);
        void Delete(int id);
        Task<IEnumerable<ClassificadosModel>> Getall();
        Task<ClassificadosModel> GetById(int id);
        Task<bool> SaveAllAsync();
    }
}