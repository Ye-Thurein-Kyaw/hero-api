using heroApi.Models;
namespace heroApi.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly HeroContext _repoContext;

        public RepositoryWrapper(HeroContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        private IHeroRepository? oHeroItem;
        public IHeroRepository HeroItem
        {
            get
            {
                if (oHeroItem == null)
                {
                    oHeroItem = new HeroRepository(_repoContext);
                }

                return oHeroItem;
            }
        }
    }
}