using Microsoft.EntityFrameworkCore;
using PulseTrain.Data;
using PulseTrain.Domain.Entities;

namespace PulseTrain.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> AnyByEmailAsync(
        string email,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext.Users.AnyAsync(
            u => u.Email.ToLower().Trim() == email.ToLower().Trim(),
            cancellationToken
        );
    }

    public async Task<User?> GetByEmailWithEstadoAsync(
        string email,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext
            .Users.Include(u => u.Estado)
            .FirstOrDefaultAsync(
                u => u.Email.ToLower().Trim() == email.ToLower().Trim(),
                cancellationToken
            );
    }

    public async Task<User?> GetByIdWithEstadoAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        return await dbContext
            .Users.Include(u => u.Estado)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}
