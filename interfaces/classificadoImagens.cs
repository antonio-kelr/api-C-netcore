using ClasificadoImagens.Models;

namespace Classificado.Interfaces
{
    public interface IClassificadoImagen
    {
        void CreateAsync(ClassificadoImagemModel classificadoImg);
        void DeleteAsync(int id);
        Task<IEnumerable<ClassificadoImagemModel>> GetAllAsync();
        Task<ClassificadoImagemModel> GetByIdAsync(int id);
        Task<bool> SaveAllAsync();



    }
}
