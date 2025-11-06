using Microsoft.EntityFrameworkCore.Query;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Models;
using System.Linq.Expressions;

namespace ShoeStore.Domain.Repositories.Shoes;

public interface IShoesRepository : IRepository<Shoe>
{
    Task<PagedList<Shoe>> GetAllAsync(
        ushort pageNumber,
        ushort pageSize,
        ICollection<string> categories,
        ICollection<string> brands,
        Expression<Func<Shoe, bool>>? predicate = null,
        Func<IQueryable<Shoe>, IIncludableQueryable<Shoe, object>>? include = null,
        Expression<Func<Shoe, object>>? sortBy = null,
        bool isSortDescending = false,
        CancellationToken cancellationToken = default);
}