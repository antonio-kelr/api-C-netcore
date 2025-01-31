using ClasificadoImagens.Models;
using Classificado.Interfaces;
using database.Data;
using Microsoft.EntityFrameworkCore;

namespace CoberturasImagens.Repositories
{
    public class ClassificaImagemdoRepositories : IClassificadoImagen
    {
        private readonly DataContext _context;

        public ClassificaImagemdoRepositories(DataContext context)
        {
            _context = context;
        }



        public void CreateAsync(ClassificadoImagemModel classificadoImg)
        {
            _context.ClassificadosImagem.Add(classificadoImg);
        }

        // Alterado para Task, para ser assíncrono
        public void DeleteAsync(int id)
        {
            var classificadoImg = _context.ClassificadosImagem.Find(id);
            if (classificadoImg != null)
            {
                _context.ClassificadosImagem.Remove(classificadoImg);
            }
        }


        public async Task<IEnumerable<ClassificadoImagemModel>> GetAllAsync()
        {
            return await _context.ClassificadosImagem.Include(ci => ci.Classificado).ToListAsync();
        }

        public async Task<ClassificadoImagemModel?> GetByIdAsync(int id)
        {
            return await _context.ClassificadosImagem.Include(ci => ci.Classificado)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
