// Repositories/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using SimpleApiServer.Data;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<IEnumerable<User>> GetAllAsync() =>
        Task.FromResult(_db.Users
            .Include(u => u.Orders)
                .ThenInclude(o => o.Products)
            .AsEnumerable());

    public async Task<User?> GetByIdAsync(int id) =>
        await _db.Users
            .Include(u => u.Orders)
                .ThenInclude(o => o.Products)
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task AddAsync(User user)
    {
        await _db.Users.AddAsync(user);
    }

    public async Task DeleteAsync(User user)
    {
        _db.Users.Remove(user);
        await Task.CompletedTask;
    }

    public async Task SaveAsync()
    {
        await _db.SaveChangesAsync();
    }
}
