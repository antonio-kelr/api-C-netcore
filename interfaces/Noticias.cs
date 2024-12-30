using Noticias.Models;

namespace Noticias.intefaces
{
    public interface IANoticias
    {
        void Create(NoticiasModel noticia);
        void UpdateNoticia(int id, NoticiasModel noticia);
        void Delete(int id);
        Task<IEnumerable<NoticiasModel>> Getall();
        Task<NoticiasModel> GetById(int id);
        Task<bool> SaveAllAsync();
    }
}