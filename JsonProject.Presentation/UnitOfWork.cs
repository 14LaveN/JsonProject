using JsonProject.Presentation;
using Microsoft.EntityFrameworkCore.Storage;
using JsonProject.Application.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace JsonProject.Persistence;

/// <summary>
/// Represents the generic unit of work.
/// </summary>
public sealed class UnitOfWork
    : IUnitOfWork
{
    private readonly BaseDbContext _baseDbContext;

    /// <summary>
    /// Initialize generic db context.
    /// </summary>
    /// <param name="baseDbContext">The base generic db context.</param>
    public UnitOfWork(BaseDbContext baseDbContext) =>
        _baseDbContext = baseDbContext;

    /// <summary>
    /// Save changes async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The integer result of saving changes.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = 0;
        var strategy = _baseDbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            result = await _baseDbContext.SaveChangesAsync(cancellationToken);
        });

        return result;
    }

    /// <summary>
    /// Begin transaction async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The db context transaction result of begin transaction.</returns>
    public async Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        IDbContextTransaction? transaction = default;
        
        var strategy = _baseDbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            transaction = await _baseDbContext.Database.BeginTransactionAsync(cancellationToken);

            return transaction;
        });
        return transaction;
    }
}