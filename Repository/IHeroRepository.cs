using heroApi.Models;

namespace heroApi.Repositories
{
    public interface IHeroRepository : IRepositoryBase<HeroItem>
    {
        Task<IEnumerable<HeroItem>> SearchHero(string searchName);
        Task<IEnumerable<HeroItem>> SearchHeroMultiple(HeroSearchPayload SearchObj);
        bool IsExists(long id);
    }
}