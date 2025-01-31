using Microsoft.EntityFrameworkCore;
using database.Data;
using Banner.intefaces;
using banner.Models;

namespace Banner.Repositories
{
    public class BannerRepositories : IBanner
    {
        private readonly DataContext _context;

        public BannerRepositories(DataContext context)
        {
            _context = context;
        }

        public void Create(BannerModel banner)
        {
            _context.Banner.Add(banner);
        }

        public async Task<IEnumerable<BannerModel>> Getall()
        {
            return await _context.Banner.ToListAsync();
        }
        public async Task<BannerModel> GetById(int id)
        {
            return await _context.Banner.FindAsync(id);
        }




        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Delete(int id)
        {
            var banner = _context.Banner.Find(id);
            if (banner != null)
            {
                _context.Banner.Remove(banner);
            }
        }
    }
}