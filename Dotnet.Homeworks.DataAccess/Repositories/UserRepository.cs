using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    public UserRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    private AppDbContext DbContext { get; }
    
    public Task<IQueryable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(DbContext.Users.AsQueryable());
    }

    public async Task<User?> GetUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        return await DbContext.Users.FirstOrDefaultAsync(user => user.Id == guid, cancellationToken);
    }

    public Task DeleteUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        DbContext.Users.Remove(new User() { Id = guid });
        return Task.CompletedTask;
    }

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        DbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task<Guid> InsertUserAsync(User user, CancellationToken cancellationToken)
    {
        return (await DbContext.Users.AddAsync(user, cancellationToken)).Entity.Id;
    }
}