using CoberturasImagens.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoberturasImagens.Interfaces
{
    public interface ICoberturaImagens
    {
        void CreateAsync(CoberturaImagemModel coberturaImagens);
        void DeleteAsync(int id);
        Task<IEnumerable<CoberturaImagemModel>> GetAllAsync();
        Task<CoberturaImagemModel> GetByIdAsync(int id);
        Task<bool> SaveAllAsync();



    }
}
