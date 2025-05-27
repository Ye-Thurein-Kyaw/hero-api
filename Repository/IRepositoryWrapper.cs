namespace heroApi.Repositories
{
    public interface IRepositoryWrapper
    {
        IHeroRepository HeroItem { get; }
        
    }
}
