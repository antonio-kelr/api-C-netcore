using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using database.Data;
using Recado.intefaces;
using Recado.Models;

namespace Recado.Repositories
{
    public class RecadoRepositories : IRecado
    {
        private readonly DataContext _context;

        public RecadoRepositories(DataContext context)
        {
            _context = context;
        }

        public void UpdateRecado(int id, RecadoModel updatedRecado)
        {
            var existingRecado = _context.Recado.Local.FirstOrDefault(a => a.Id == id)
                                ?? _context.Recado.Find(id);

            if (existingRecado != null)
            {
                // Atualize somente os campos que podem ser modificados
                _context.Entry(existingRecado).CurrentValues.SetValues(updatedRecado);

                // Garante que o ID não será alterado
                _context.Entry(existingRecado).Property(e => e.Id).IsModified = false;
            }
        }

        public void Create(RecadoModel Recado)
        {
            _context.Recado.Add(Recado);
        }

        public async Task<IEnumerable<RecadoModel>> Getall()
        {
            return await _context.Recado.ToListAsync();
        }

        public async Task<RecadoModel> GetById(int id)
        {
            return await _context.Recado.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Delete(int id)
        {
            var Recado = _context.Recado.Find(id);
            if (Recado != null)
            {
                _context.Recado.Remove(Recado);
            }
        }
    }
}