using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext db) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync() =>
        await db.Users
            .Include(u => u.Subscription)
            .ToListAsync();

    public async Task<User?> GetByIdAsync(int id) =>
        await db.Users
            .Include(u => u.Subscription)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<IEnumerable<User>> GetBySubscriptionTypeAsync(string subscriptionType) =>
        await db.Users
            .Include(u => u.Subscription)
            .Where(u => u.Subscription.Type.ToString() == subscriptionType)
            .ToListAsync();
    
    public async Task AddAsync(User user)
    {
        await db.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        db.Users.Update(user);
    }

    public void Remove(User user)
    {
        db.Users.Remove(user);
    }

    public Task SaveChangesAsync() =>
        db.SaveChangesAsync();
}