using PulseTrain.Domain.Entities;

namespace PulseTrain.Infrastructure.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> AnyByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailWithEstadoAsync(
        string email,
        CancellationToken cancellationToken = default
    );
    Task<User?> GetByIdWithEstadoAsync(int id, CancellationToken cancellationToken = default);
}
