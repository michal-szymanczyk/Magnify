using System.Collections.Concurrent;

namespace MagnifyLogistics.Persistence.InMemory
{
    internal class InMemoryUniOfWork
    {
        // simple mock of database context

        private readonly ConcurrentDictionary<Type, List<BaseModel>> _data;

        public InMemoryUniOfWork()
        {
            _data = new ConcurrentDictionary<Type, List<BaseModel>>();
        }

        public IQueryable<BaseModel> GetQuery<T>() where T : BaseModel
            => Table<T>().AsQueryable();

        public IList<BaseModel> Table<T>() where T : BaseModel
            => _data.GetOrAdd(typeof(T), _ => new List<BaseModel>());
    }
}
