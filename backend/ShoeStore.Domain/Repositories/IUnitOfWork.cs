using ShoeStore.Domain.Entities.Carts;
using ShoeStore.Domain.Entities.Orders;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Entities.Users;
using ShoeStore.Domain.Repositories.Orders;
using ShoeStore.Domain.Repositories.Shoes;
using ShoeStore.Domain.Repositories.Users;

namespace ShoeStore.Domain.Repositories;

public interface IUnitOfWork
{
    IRepository<ShoppingCart> ShoppingCarts { get; }
    IRepository<CartItem> CartItems { get; }
    IOrderRepository Orders { get; }
    IRepository<DeliveryMethod> DeliveryMethods { get; }
    IRepository<Brand> Brands { get; }
    IRepository<Category> Categories { get; }
    IShoesRepository Shoes { get; }
    IRepository<ShoeImage> ShoeImages { get; }
    IRepository<Address> Addresses { get; }
    IRepository<RefreshToken> RefreshTokens { get; }
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}