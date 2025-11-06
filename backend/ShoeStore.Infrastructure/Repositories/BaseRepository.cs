using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Domain.Models;
using ShoeStore.Domain.Repositories;
using System.Linq.Expressions;
using ShoeStore.Infrastructure.Data;

namespace ShoeStore.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly ShoeStoreDbContext DbContext;

    public BaseRepository(ShoeStoreDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public TEntity Add(TEntity entity)
    {
        return DbContext.Set<TEntity>().Add(entity).Entity;
    }

    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken cancellationToken = default)
    {
        return await GetQueryable(predicate, include)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> GetAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        return await GetQueryable(predicate)
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> GetSingleAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken cancellationToken = default)
    {
        return await GetQueryable(predicate, include)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        CancellationToken cancellationToken = default)
    {
        return await GetQueryable(predicate, include)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedList<TEntity>> GetAllAsync(
        ushort pageNumber,
        ushort pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Expression<Func<TEntity, object>>? sortBy = null,
        bool isSortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var query = GetQueryable(
            predicate,
            include,
            sortBy,
            isSortDescending);

        var count = (ushort)await query.CountAsync(cancellationToken);
        pageSize = Math.Min(pageSize, count);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<TEntity>(items, pageNumber, pageSize, count);
    }

    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>().AnyAsync(predicate, cancellationToken);
    }

    public async Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<TEntity>().AnyAsync(cancellationToken);
    }

    protected IQueryable<TEntity> GetQueryable(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        Expression<Func<TEntity, object>>? sortBy = null,
        bool isSortDescending = false)
    {
        var query = DbContext.Set<TEntity>().AsNoTracking();

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (include is not null)
        {
            query = include(query);
        }

        if (sortBy is not null)
        {
            query = isSortDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);
        }

        return query;
    }
}