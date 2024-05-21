using System.Threading.Tasks;

namespace Core.Interfaces.Database
{
    public interface IUnitOfWork
    {
        int SaveChanges();

        Task<int> SaveChangesAsync();
        Task Dispose();
    }
}
