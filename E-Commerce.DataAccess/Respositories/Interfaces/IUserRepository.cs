using E_Commerce.Domain.Entities;

namespace E_Commerce.DataAccess.Respositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(); // Gets all data.
        Task<User> GetByIdAsync(string id);      // Gets data by ID.
        Task CreateAsync(User entity);        // Create a new resource.
        Task UpdateAsync(User entity);        // Updates existing data.
        Task DeleteByIdAsync(string id);      // Deletes data by ID.
    }
}
