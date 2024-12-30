using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using database.Data;
using Noticias.Models;
using Noticias.intefaces;

namespace Noticias.Repositories
{
    public class NoticiasRepositories : IANoticias
    {
        private readonly DataContext _context;

        public NoticiasRepositories(DataContext context)
        {
            _context = context;
        }

        public void UpdateNoticia(int id, NoticiasModel updatedNoticia)
        {
            var existingNoticia = _context.Noticias.Local.FirstOrDefault(a => a.Id == id)
                                ?? _context.Noticias.Find(id);

            if (existingNoticia != null)
            {
                // Atualize somente os campos alterados
                _context.Entry(existingNoticia).CurrentValues.SetValues(updatedNoticia);
            }
        }

        public void Create(NoticiasModel noticia)
        {
            _context.Noticias.Add(noticia);
        }

        public async Task<IEnumerable<NoticiasModel>> Getall()
        {
            return await _context.Noticias.ToListAsync();
        }

        public async Task<NoticiasModel> GetById(int id)
        {
            return await _context.Noticias.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Delete(int id)
        {
            var noticias = _context.Noticias.Find(id);
            if (noticias != null)
            {
                _context.Noticias.Remove(noticias);
            }
        }
    }
}