using Microsoft.Extensions.Logging;
using ShoeStore.Domain.Enums;
using System.Reflection;
using System.Text.Json;
using ShoeStore.Application.Interfaces;
using ShoeStore.Domain.Entities.Orders;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Entities.Users;
using ShoeStore.Domain.Exceptions;
using ShoeStore.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ShoeStore.Infrastructure.Data.Initializers;

public class DbInitializer : IInitializer
{
    public record UserRegisterDto(
        Guid UserId,
        string FirstName,
        string LastName,
        string Email,
        string PhoneNumber,
        string Password,
        Address? Address);

    private readonly ShoeStoreDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IImageService _imageService;
    private readonly ILogger<DbInitializer> _logger;
    private readonly string _seedDataPath;

    public DbInitializer(
        ShoeStoreDbContext context,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IImageService imageService,
        ILogger<DbInitializer> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("Unable to determine the current path.");
        _seedDataPath = Path.Combine(path, "Data", "Initializers", "SeedData");
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Seeding database...");
        await SeedUsersAsync(cancellationToken);

        await SeedBrandsAsync(cancellationToken);
        await SeedCategoriesAsync(cancellationToken);
        await SeedShoesAsync(cancellationToken);

        await SeedDeliveryMethodsAsync(cancellationToken);
        await SeedOrdersAsync(cancellationToken);

        _logger.LogInformation("Database seeded successfully.");
    }

    private async Task<Role[]> SeedRolesAsync(CancellationToken cancellationToken)
    {
        if (await _context.Roles.AnyAsync(cancellationToken))
        {
            return await _context.Roles.ToArrayAsync(cancellationToken);
        }

        Role[] roles =
        [
            new() { RoleId = Guid.Parse("a3f4e3d2-1b2c-4e5f-9a7b-2c6d5e4f3a1b"), RoleName = RoleType.Admin.ToString() },
            new() { RoleId = Guid.Parse("b5d3a1f2-6c7d-4a8b-9e1f-3c2b4d5a6e7f"), RoleName = RoleType.Customer.ToString() }
        ];

        foreach (var role in roles)
        {
            _context.Roles.Add(role);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return roles;
    }

    private async Task SeedBrandsAsync(CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Brands.AnyAsync(cancellationToken))
        {
            return;
        }

        var brands = JsonSerializer.Deserialize<List<Brand>>(
            await File.ReadAllTextAsync($"{_seedDataPath}/brands.json", cancellationToken));

        if (brands == null || brands.Count == 0)
        {
            return;
        }

        foreach (var brand in brands)
        {
            _unitOfWork.Brands.Add(brand);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedCategoriesAsync(CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Categories.AnyAsync(cancellationToken))
        {
            return;
        }

        var categories = JsonSerializer.Deserialize<List<Category>>(
            await File.ReadAllTextAsync($"{_seedDataPath}/categories.json", cancellationToken));

        if (categories == null || categories.Count == 0)
        {
            return;
        }

        foreach (var category in categories)
        {
            _unitOfWork.Categories.Add(category);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedShoesAsync(CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Shoes.AnyAsync(cancellationToken))
        {
            return;
        }

        var shoes = JsonSerializer.Deserialize<List<Shoe>>(
            await File.ReadAllTextAsync($"{_seedDataPath}/shoes.json", cancellationToken));

        if (shoes == null || shoes.Count == 0)
        {
            return;
        }

        foreach (var shoe in shoes)
        {
            var images = shoe.ShoeImages.ToList();

            shoe.ShoeImages.Clear();

            await UploadImagesAsync(shoe, images);

            _unitOfWork.Shoes.Add(shoe);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task UploadImagesAsync(Shoe shoe, IList<ShoeImage> images)
    {
        foreach (var image in images)
        {
            var publicId = image.PublicId;

            var imageRelativePath = image.Url.Replace("/", Path.DirectorySeparatorChar.ToString())
                .Replace("\\", Path.DirectorySeparatorChar.ToString());

            var fullImagePath = Path.Combine(_seedDataPath, imageRelativePath);

            if (!File.Exists(fullImagePath))
            {
                _logger.LogWarning("Image file not found: {FullImagePath}", fullImagePath);
                continue;
            }

            await using var stream = new FileStream(fullImagePath, FileMode.Open, FileAccess.Read);

            var formFile = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(fullImagePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            var imageUrl = await _imageService.UploadAsync(publicId, formFile);

            var shoeImage = new ShoeImage
            {
                PublicId = publicId,
                Url = imageUrl,
                IsMain = image.IsMain,
                ShoeId = shoe.ShoeId
            };

            shoe.ShoeImages.Add(shoeImage);
        }
    }

    private async Task SeedOrdersAsync(CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Orders.AnyAsync(cancellationToken))
        {
            return;
        }

        var orders = JsonSerializer.Deserialize<List<Order>>(
            await File.ReadAllTextAsync($"{_seedDataPath}/orders.json", cancellationToken));

        if (orders == null || orders.Count == 0)
        {
            return;
        }

        foreach (var order in orders)
        {
            order.OrderItems = order.OrderItems
                .GroupBy(x => new { x.OrderId, x.ShoeId })
                .Select(g => g.First())
                .ToList();

            order.Subtotal = order.OrderItems.Sum(x => x.Price * x.Quantity);

            var delivery = await _unitOfWork.DeliveryMethods.GetSingleAsync(
                x => x.DeliveryMethodId == order.DeliveryMethodId,
                cancellationToken: cancellationToken)
                ?? throw new NotFoundException($"Delivery Method with id ${order.DeliveryMethodId} not found");

            order.Shipping = delivery.Price;

            foreach (var orderItem in order.OrderItems)
            {
                orderItem.OrderId = order.OrderId;
            }

            _unitOfWork.Orders.Add(order);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedDeliveryMethodsAsync(CancellationToken cancellationToken)
    {
        if (await _unitOfWork.DeliveryMethods.AnyAsync(cancellationToken))
        {
            return;
        }

        var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(
            await File.ReadAllTextAsync($"{_seedDataPath}/delivery.json", cancellationToken));

        if (deliveryMethods == null || deliveryMethods.Count == 0)
        {
            return;
        }

        foreach (var deliveryMethod in deliveryMethods)
        {
            _unitOfWork.DeliveryMethods.Add(deliveryMethod);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedUsersAsync(CancellationToken cancellationToken)
    {
        if (await _unitOfWork.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var roles = await SeedRolesAsync(cancellationToken);

        await SeedAdminsAsync(roles, cancellationToken);
        await SeedCustomersAsync(roles, cancellationToken);
    }

    private async Task SeedAdminsAsync(Role[] roles, CancellationToken cancellationToken)
    {
        var admins = JsonSerializer.Deserialize<List<UserRegisterDto>>(
            await File.ReadAllTextAsync($"{_seedDataPath}/employees.json", cancellationToken));

        if (admins == null || admins.Count == 0)
        {
            return;
        }

        foreach (var admin in admins)
        {
            var user = new User
            {
                UserId = admin.UserId,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                PasswordHash = _passwordHasher.Hash(admin.Password),
                PhoneNumber = admin.PhoneNumber
            };

            user.RefreshToken = new RefreshToken
            {
                UserId = user.UserId,
            };

            var entity = _unitOfWork.Users.Add(user);

            entity.Roles.Add(roles.First(x => x.RoleName == RoleType.Admin.ToString()));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedCustomersAsync(Role[] roles, CancellationToken cancellationToken)
    {
        var customers = JsonSerializer.Deserialize<List<UserRegisterDto>>(
            await File.ReadAllTextAsync($"{_seedDataPath}/customers.json", cancellationToken));

        if (customers == null || customers.Count == 0)
        {
            return;
        }

        foreach (var customer in customers)
        {
            var user = new User
            {
                UserId = customer.UserId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PasswordHash = _passwordHasher.Hash(customer.Password),
                PhoneNumber = customer.PhoneNumber
            };

            if (customer.Address != null)
            {
                customer.Address.UserId = user.UserId;
                user.Address = customer.Address;
            }

            user.RefreshToken = new RefreshToken
            {
                UserId = user.UserId,
            };

            var entity = _unitOfWork.Users.Add(user);

            entity.Roles.Add(roles.First(x => x.RoleName == RoleType.Customer.ToString()));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}