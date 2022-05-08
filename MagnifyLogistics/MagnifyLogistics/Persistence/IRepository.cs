namespace MagnifyLogistics.Persistence
{
    public interface IRepository<T>
        where T : BaseModel
    {
        // IQueryable<T> GetQuery();
        // to avoid concurrency issues (because we are not using any ORM), only Find method is provided instead of GetQuery
        IList<T> Find(Func<T, bool> query);
        void Add(T model);
        void Update(T model);
    }
}
