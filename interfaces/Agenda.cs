using Agenda.Models;

namespace Agenda.intefaces
{
    public interface IAgenda
    {
        void Create(AgendaModel agenda);
        void UpdateAgenda(int id, AgendaModel agenda);
        void Delete(int id);
        Task<IEnumerable<AgendaModel>> Getall();
        Task<AgendaModel> GetById(int id);
        Task<bool> SaveAllAsync();
    }
}