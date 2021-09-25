using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task<int> SaveAsync();
    }
}
