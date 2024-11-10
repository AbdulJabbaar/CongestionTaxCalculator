using CongestionTaxCalculator.Application.Repositories;
using CongestionTaxCalculator.Infrastructure.Persistence;

namespace CongestionTaxCalculator.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly InMemoryDbContext _dbContext;

    public UnitOfWork(InMemoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitChangeAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
