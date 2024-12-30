using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using database.Data;
using Agenda.Models;
using Agenda.intefaces;

namespace Agenda.Repositories
{
    public class AgendaRepositories : IAgenda
    {
        private readonly DataContext _context;

        public AgendaRepositories(DataContext context)
        {
            _context = context;
        }

        public void UpdateAgenda(int id, AgendaModel updatedAgenda)
        {
            var existingAgenda = _context.Agenda.Local.FirstOrDefault(a => a.Id == id)
                                ?? _context.Agenda.Find(id);

            if (existingAgenda != null)
            {
                // Atualize somente os campos alterados
                _context.Entry(existingAgenda).CurrentValues.SetValues(updatedAgenda);
            }
        }

        public void Create(AgendaModel agenda)
        {
            _context.Agenda.Add(agenda);
        }

        public async Task<IEnumerable<AgendaModel>> Getall()
        {
            return await _context.Agenda.ToListAsync();
        }

        public async Task<AgendaModel> GetById(int id)
        {
            return await _context.Agenda.FindAsync(id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Delete(int id)
        {
            var agenda = _context.Agenda.Find(id);
            if (agenda != null)
            {
                _context.Agenda.Remove(agenda);
            }
        }
    }
}