﻿using JsonProject.Application.Core.Abstractions;
using JsonProject.Application.Core.Abstractions.Messaging;
using MediatR;
using JsonProject.Application.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using IUnitOfWork = JsonProject.Application.Core.Abstractions.IUnitOfWork;

namespace JsonProject.Application.Core.Behaviours;

/// <summary>
/// Represents the generic transaction behaviour class.
/// </summary>
public sealed class BaseTransactionBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : class
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDbContext _userDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTransactionBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The user unit of work.</param>
    /// <param name="userDbContext">The user database context.</param>
    public BaseTransactionBehavior(
        IUnitOfWork unitOfWork,
        IDbContext userDbContext)
    {
        _unitOfWork = unitOfWork;
        _userDbContext = userDbContext;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is IQuery<TResponse>)
        {
            return await next();
        }
        
        var strategy = _userDbContext.EfDatabase.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                TResponse response = await next();

                await transaction!.CommitAsync(cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return response;
            }
            catch (Exception)
            {
                await transaction!.RollbackAsync(cancellationToken);

                throw;
            }
        });

        throw new ArgumentException();
    }
}