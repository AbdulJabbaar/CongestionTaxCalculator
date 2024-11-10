namespace CongestionTaxCalculator.Application.Repositories;

public interface IUnitOfWork
{
    public Task CommitChangeAsync(CancellationToken cancellationToken = default);
}
