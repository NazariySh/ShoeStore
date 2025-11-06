using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Models;
using ShoeStore.Domain.Repositories.Shoes;
using ShoeStore.Infrastructure.Data;
using static Dapper.SqlMapper;

namespace ShoeStore.Infrastructure.Repositories.Shoes;

public class ShoesRepository : BaseRepository<Shoe>, IShoesRepository
{
    public ShoesRepository(ShoeStoreDbContext dbContext)
        : base(dbContext)
    {
    }


    public async Task<PagedList<Shoe>> GetAllAsync(
        ushort pageNumber,
        ushort pageSize,
        ICollection<string> categories,
        ICollection<string> brands,
        Expression<Func<Shoe, bool>>? predicate = null,
        Func<IQueryable<Shoe>, IIncludableQueryable<Shoe, object>>? include = null,
        Expression<Func<Shoe, object>>? sortBy = null,
        bool isSortDescending = false,
        CancellationToken cancellationToken = default)
    {
        var query = GetQueryable(predicate, include, sortBy, isSortDescending);

        if (categories.Count > 0)
        {
            query = query.Where(x => categories.Contains(x.Category.Name));
        }

        if ( brands.Count > 0)
        {
            query = query.Where(x => brands.Contains(x.Brand.Name));
        }

        var count = (ushort)await query.CountAsync(cancellationToken);
        pageSize = Math.Min(pageSize, count);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync(cancellationToken);

        return new PagedList<Shoe>(items, pageNumber, pageSize, count);
    }
}