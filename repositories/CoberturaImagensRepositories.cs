using CoberturasImagens.Interfaces;
using CoberturasImagens.Models;
using database.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoberturasImagens.Repositories
{
    public class CoberturaImagensRepository : ICoberturaImagens
    {
        private readonly DataContext _context;

        public CoberturaImagensRepository(DataContext context)
        {
            _context = context;
        }



        public void CreateAsync(CoberturaImagemModel coberturaImagens)
        {
            _context.CoberturaImagens.Add(coberturaImagens);
        }

        // Alterado para Task, para ser assíncrono
        public void DeleteAsync(int id)
        {
            var coberturaImagem = _context.CoberturaImagens.Find(id);
            if (coberturaImagem != null)
            {
                _context.CoberturaImagens.Remove(coberturaImagem);
            }
        }


        public async Task<IEnumerable<CoberturaImagemModel>> GetAllAsync()
        {
            return await _context.CoberturaImagens.Include(ci => ci.Cobertura).ToListAsync();
        }

        public async Task<CoberturaImagemModel> GetByIdAsync(int id)
        {
            return await _context.CoberturaImagens.Include(ci => ci.Cobertura)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
