using API.Domains;
using API.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<T>)repositories[type];
        }
    }
}
