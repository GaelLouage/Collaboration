using MongoDB.Bson;
using System.Linq.Expressions;

namespace Collaboration.Repositories.Interfaces
{
    public interface IMongoRepository<T>
    {

        Task InsertAsync(T item);
        Task<T> GetByIdAsync(ObjectId id);
        Task<IEnumerable<T>> GetByFilterAsync(Expression<Func<T, bool>> filter);
        Task UpdateAsync(ObjectId id, T item);
        Task DeleteAsync(ObjectId id);
        Task UpdateByUsernameAsync(string username, T item);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByUserNameAsync(string name);
    }
}
