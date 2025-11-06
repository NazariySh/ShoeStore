using Microsoft.Extensions.Configuration;
using ShoeStore.Domain.Entities.Carts;
using ShoeStore.Domain.Entities.Orders;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Entities.Users;
using ShoeStore.Domain.Repositories;
using ShoeStore.Domain.Repositories.Orders;
using ShoeStore.Domain.Repositories.Shoes;
using ShoeStore.Domain.Repositories.Users;
using ShoeStore.Infrastructure.Data;
using ShoeStore.Infrastructure.Repositories.Orders;
using ShoeStore.Infrastructure.Repositories.Shoes;
using ShoeStore.Infrastructure.Repositories.Users;

namespace ShoeStore.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ShoeStoreDbContext _dbContext;
    private readonly IConfiguration _configuration;

    private IRepository<ShoppingCart>? _shoppingCartRepository;
    private IRepository<CartItem>? _cartItemRepository;
    private IOrderRepository? _orderRepository;
    private IRepository<DeliveryMethod>? _deliveryMethodRepository;
    private IRepository<Brand>? _brandRepository;
    private IRepository<Category>? _categoryRepository;
    private IShoesRepository? _shoesRepository;
    private IRepository<ShoeImage>? _shoeImageRepository;
    private IRepository<Address>? _addressRepository;
    private IRepository<RefreshToken>? _refreshTokenRepository;
    private IUserRepository? _userRepository;

    public UnitOfWork(ShoeStoreDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public IRepository<ShoppingCart> ShoppingCarts =>
        _shoppingCartRepository ??= new BaseRepository<ShoppingCart>(_dbContext);

    public IRepository<CartItem> CartItems =>
        _cartItemRepository ??= new BaseRepository<CartItem>(_dbContext);

    public IOrderRepository Orders =>
        _orderRepository ??= new OrderRepository(_dbContext, _configuration);

    public IRepository<DeliveryMethod> DeliveryMethods =>
        _deliveryMethodRepository ??= new BaseRepository<DeliveryMethod>(_dbContext);

    public IRepository<Brand> Brands =>
        _brandRepository ??= new BaseRepository<Brand>(_dbContext);

    public IRepository<Category> Categories =>
        _categoryRepository ??= new BaseRepository<Category>(_dbContext);

    public IShoesRepository Shoes =>
        _shoesRepository ??= new ShoesRepository(_dbContext);

    public IRepository<ShoeImage> ShoeImages =>
        _shoeImageRepository ??= new BaseRepository<ShoeImage>(_dbContext);

    public IRepository<Address> Addresses =>
        _addressRepository ??= new BaseRepository<Address>(_dbContext);

    public IRepository<RefreshToken> RefreshTokens =>
        _refreshTokenRepository ??= new BaseRepository<RefreshToken>(_dbContext);

    public IUserRepository Users =>
        _userRepository ??= new UserRepository(_dbContext, _configuration);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}