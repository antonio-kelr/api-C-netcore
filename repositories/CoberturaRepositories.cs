using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using database.Data;
using Coberturas.Models;
using Coberturas.intefaces;

namespace Coberturas.Repositories
{
    public class CoberturaRepositories : IACoberturas
    {
        private readonly DataContext _context;

        public CoberturaRepositories(DataContext context)
        {
            _context = context;
        }

        public void UpdateCobertura(int id, CoberturaModel updatedCobertura)
        {
            var existingCobertura = _context.Coberturas.Local.FirstOrDefault(a => a.Id == id)
                                ?? _context.Coberturas.Find(id);

            if (existingCobertura != null)
            {
                // Atualize somente os campos alterados
                _context.Entry(existingCobertura).CurrentValues.SetValues(updatedCobertura);
            }
        }

        public void Create(CoberturaModel cobertura)
        {
            _context.Coberturas.Add(cobertura);
        }

           public async Task<IEnumerable<CoberturaModel>> Getall()
        {
            return await _context.Coberturas
                                 .Include(c => c.Imagens)
                                 .ToListAsync();
        }

        public async Task<CoberturaModel> GetById(int id)
        {
            return await _context.Coberturas.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Delete(int id)
        {
            var cobertura = _context.Coberturas.Find(id);
            if (cobertura != null)
            {
                _context.Coberturas.Remove(cobertura);
            }
        }
    }
}