using AutoMapper;

namespace MagnifyLogistics.Persistence.InMemory
{
    internal class Repository<T> : IRepository<T>
        where T: BaseModel
    {
        // Using EF core, another abstraction over DBContext (instead of InMemoryDataStore) can be created so this class could be reusable with different ORMs
        // for simplicity of that exercise it's a concrete implementation dependent on InMemoryDataStore

        private readonly InMemoryUniOfWork _dataStore;
        private readonly IMapper _mapper;

        // for simplicity (assuming that each Repository instance is registered as singleton in DI container),
        // very simple locking mechanism, in normal case it should be handled within ORM
        private readonly object _updateLock = new();

        public Repository(InMemoryUniOfWork dataStore, IMapper mapper)
        {
            _mapper = mapper;
            _dataStore = dataStore;
        }

        public IList<T> Find(Func<T, bool> query)
        {
            var elements = _dataStore.GetQuery<T>().Where(m => query((T)m)).ToList();

            // without an ORM (which could handle db changes), copy of internal memory objects are returned, for simplicity AutoMapper is used
            return _mapper.Map<List<T>>(elements);
        }

        public void Add(T model)
        {
            // for simplicity, Id constraints are not validated

            var entity = _mapper.Map<T>(model); // again, to block access to internally stored objects, copy of them is inserted into the store
            var table = _dataStore.Table<T>();

            lock (_updateLock)
            {
                entity.Id = table.Count;
                table.Add(entity);
            }
        }

        public void Update(T model)
        {
            // for simplicity, Id constraints are not validated

            var entity = _dataStore.GetQuery<T>().SingleOrDefault(m => m.Id == model.Id);

            if (entity != null)
            {
                lock (_updateLock) // very poor locking mechanism, only to highlight the problem when not using an ORM
                {
                    _mapper.Map(model, entity); //  similar to above methods, to block access to internally stored objects
                }
            }
            
        }
    }
}
