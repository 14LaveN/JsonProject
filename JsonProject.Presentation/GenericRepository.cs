﻿using JsonProject.Domain.Common.Core.Primitives;
using JsonProject.Domain.Common.Core.Primitives.Maybe;
using JsonProject.Domain.Common.Core.Primitives.Result;

namespace JsonProject.Presentation;

/// <summary>
/// Represents the generic repository with the most common repository methods.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class GenericRepository<TEntity>
    where TEntity : Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{TEntity}"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    protected GenericRepository(BaseDbContext dbContext) =>
        DbContext = dbContext;

    /// <summary>
    /// Gets the database context.
    /// </summary>
    protected BaseDbContext DbContext { get; }

    /// <summary>
    /// Gets the entity with the specified identifier.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    /// <returns>The maybe instance that may contain the entity with the specified identifier.</returns>
    public async Task<Maybe<TEntity>> GetByIdAsync(Guid id) 
        => await DbContext.GetByIdAsync<TEntity>(id);

    /// <summary>
    /// Inserts the specified entity into the database.
    /// </summary>
    /// <param name="entity">The entity to be inserted into the database.</param>
    public async Task<Result> Insert(TEntity entity)
    {
        await DbContext.Insert(entity);

        return await Result.Success();
    }

    /// <summary>
    /// Inserts the specified entities to the databases.
    /// </summary>
    /// <param name="entities">The entities to be inserted into the database.</param>
    public async Task InsertRange(IReadOnlyCollection<TEntity> entities) 
        => await DbContext.InsertRange(entities);

    /// <summary>
    /// Updates the specified entity in the database.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

    /// <summary>
    /// Removes the specified entity from the database.
    /// </summary>
    /// <param name="entity">The entity to be removed from the database.</param>
    public async Task Remove(TEntity entity) => await DbContext.Remove(entity);
}