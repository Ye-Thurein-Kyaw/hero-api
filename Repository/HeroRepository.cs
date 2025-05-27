using System.Data;
using System.Linq;
using heroApi.Models;
using Microsoft.EntityFrameworkCore;

namespace heroApi.Repositories
{
    public class HeroRepository : RepositoryBase<HeroItem>, IHeroRepository
    {
        public HeroRepository(HeroContext repositoryContext) : base(repositoryContext) { }

        public async Task<IEnumerable<HeroItem>> SearchHero(string searchTerm)
        {
            return await RepositoryContext.HeroItems
                        .Where(s => s.Name.Contains(searchTerm))
                        .OrderBy(s => s.Id).ToListAsync();
        }

        public async Task<IEnumerable<HeroItem>> SearchHeroMultiple(HeroSearchPayload SearchObj)
        {
            return await RepositoryContext.HeroItems
                        .Where(s => s.Name.Contains(SearchObj.NameTerm ?? "") || s.Address.Contains(SearchObj.AddressTerm ?? ""))
                        .OrderBy(s => s.Id).ToListAsync();
        }

        public bool IsExists(long id)
        {
            return RepositoryContext.HeroItems.Any(e => e.Id == id);
        }
    }

}