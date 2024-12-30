using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using database.Data;
using Cadastro.Models;
using Cadastro.intefaces;
using BCrypt.Net;

namespace Cadastro.Repositories
{
    public class CadastroRepositories : IACadastro
    {
        private readonly DataContext _context;

        public CadastroRepositories(DataContext context)
        {
            _context = context;
        }

        // Método para criar um novo cadastro
        public async Task Create(CadastroModel cadastro)
        {
            // Criptografar a senha antes de salvar
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(cadastro.Senha);
            cadastro.Senha = hashedPassword;

            // Adicionar o cadastro no banco de dados
            _context.Cadastro.Add(cadastro);
            await _context.SaveChangesAsync();
        }

        // Método para recuperar todos os cadastros
        public async Task<IEnumerable<CadastroModel>> Getall()
        {
            return await _context.Cadastro.ToListAsync();
        }
    }
}
