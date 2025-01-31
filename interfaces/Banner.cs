using banner.Models;

namespace Banner.intefaces
{
    public interface IBanner
    {
        void Create(BannerModel banner);
        void Delete(int id);
        Task<IEnumerable<BannerModel>> Getall();
        Task<BannerModel> GetById(int id);

        Task<bool> SaveAllAsync();
    }
}