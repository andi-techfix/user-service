using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetBySubscriptionTypeAsync(string subscriptionType);
    Task AddAsync(User user);
    void Update(User user);
    void Remove(User user);
    Task SaveChangesAsync();
}