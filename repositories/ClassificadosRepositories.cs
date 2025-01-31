using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using database.Data;
using Classificados.Models;
using Classificados.intefaces;

namespace Classificados.Repositories
{
    public class ClassificadoRepositories : IClassificados
    {
        private readonly DataContext _context;


        public ClassificadoRepositories(DataContext context)
        {
            _context = context;
        }

        public void Create(ClassificadosModel classificado)
        {
            _context.Classificados.Add(classificado);
        }



        public void Update(int id, ClassificadosModel updatedclassificado)
        {
            var existingclassificado = _context.Classificados.Local.FirstOrDefault(a => a.Id == id)
                                    ?? _context.Classificados.Find(id);

            if (existingclassificado != null)
            {
                // Atualize os campos
                _context.Entry(existingclassificado).CurrentValues.SetValues(updatedclassificado);

                // Garante que o ID não será alterado
                _context.Entry(existingclassificado).Property(e => e.Id).IsModified = false;

                // Se necessário, force as propriedades a serem marcadas como modificadas
                _context.Entry(existingclassificado).State = EntityState.Modified;
            }
        }


        public async Task<IEnumerable<ClassificadosModel>> Getall()
        {
            return await _context.Classificados
                                 .Include(c => c.Imagens)
                                 .ToListAsync();
        }

        public async Task<ClassificadosModel> GetById(int id)
        {
            return await _context.Classificados.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Delete(int id)
        {
            var classificado = _context.Classificados.Find(id);
            if (classificado != null)
            {
                _context.Classificados.Remove(classificado);
            }
        }
    }
}