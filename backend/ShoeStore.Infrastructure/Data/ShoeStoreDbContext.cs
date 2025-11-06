using Microsoft.EntityFrameworkCore;
using ShoeStore.Domain.Entities.Carts;
using ShoeStore.Domain.Entities.Orders;
using ShoeStore.Domain.Entities.Shoes;
using ShoeStore.Domain.Entities.Users;
using ShoeStore.Domain.Entities;

namespace ShoeStore.Infrastructure.Data;

public partial class ShoeStoreDbContext : DbContext
{
    public ShoeStoreDbContext()
    {
    }

    public ShoeStoreDbContext(DbContextOptions<ShoeStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActionLog> ActionLogs { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shoe> Shoes { get; set; }

    public virtual DbSet<ShoeImage> ShoeImages { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActionLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__ActionLo__5E5486484E49DBBE");

            entity.ToTable("ActionLog");

            entity.Property(e => e.LogId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ActionTime)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ActionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.ActionLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ActionLog__UserI__02FC7413");
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Addresse__1788CC4CA3AB27A5");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Country).HasMaxLength(50);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.Street).HasMaxLength(100);

            entity.HasOne(d => d.User).WithOne(p => p.Address)
                .HasForeignKey<Address>(d => d.UserId)
                .HasConstraintName("FK__Addresses__UserI__3D5E1FD2");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brands__DAD4F05ED9D21577");

            entity.HasIndex(e => e.Name, "UQ__Brands__737584F625D72F35").IsUnique();

            entity.Property(e => e.BrandId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => new { e.ShoppingCartId, e.ProductId }).HasName("PK__CartItem__B1385688A8BEA3C3");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CartItems__Produ__7D439ABD");

            entity.HasOne(d => d.ShoppingCart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ShoppingCartId)
                .HasConstraintName("FK__CartItems__Shopp__7C4F7684");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B12EA7316");

            entity.HasIndex(e => e.Name, "UQ__Categori__737584F6EFC30262").IsUnique();

            entity.Property(e => e.CategoryId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<DeliveryMethod>(entity =>
        {
            entity.HasKey(e => e.DeliveryMethodId).HasName("PK__Delivery__7B03A042709B9EFD");

            entity.HasIndex(e => e.Name, "UQ__Delivery__737584F64369EEC5").IsUnique();

            entity.Property(e => e.DeliveryMethodId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BCF6AB1D80F");

            entity.ToTable(tb => tb.HasTrigger("trg_Orders_Audit"));

            entity.HasIndex(e => e.CreatedAt, "IX_Orders_CreatedAt");

            entity.Property(e => e.OrderId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Shipping).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderCustomers)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Orders__Customer__6D0D32F4");

            entity.HasOne(d => d.DeliveryMethod).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DeliveryMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__Delivery__6EF57B66");

            entity.HasOne(d => d.Employee).WithMany(p => p.OrderEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Orders__Employee__6E01572D");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ShoeId }).HasName("PK__OrderIte__86386E70D69CBB1E");

            entity.ToTable(tb =>
            {
                tb.HasTrigger("trg_CheckStockBeforeInsert");
                tb.HasTrigger("trg_UpdateTotalSold");
            });

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__Order__73BA3083");

            entity.HasOne(d => d.Shoe).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ShoeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__ShoeI__74AE54BC");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__RefreshT__1788CC4CD99C320A");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.ExpiryTime).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.User).WithOne(p => p.RefreshToken)
                .HasForeignKey<RefreshToken>(d => d.UserId)
                .HasConstraintName("FK__RefreshTo__UserI__403A8C7D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A71DB492E");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B61607CD24D48").IsUnique();

            entity.Property(e => e.RoleId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Shoe>(entity =>
        {
            entity.HasKey(e => e.ShoeId).HasName("PK__Shoes__5A835BF501C23F70");

            entity.ToTable(tb => tb.HasTrigger("trg_Shoes_Audit"));

            entity.HasIndex(e => new { e.CategoryId, e.BrandId }, "IX_Shoes_CategoryId_BrandId");

            entity.HasIndex(e => e.Name, "IX_Shoes_Name");

            entity.HasIndex(e => e.Sku, "UQ__Shoes__CA1FD3C51D3F5857").IsUnique();

            entity.Property(e => e.ShoeId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Brand).WithMany(p => p.Shoes)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shoes__BrandId__59FA5E80");

            entity.HasOne(d => d.Category).WithMany(p => p.Shoes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shoes__CategoryI__59063A47");
        });

        modelBuilder.Entity<ShoeImage>(entity =>
        {
            entity.HasKey(e => e.ShoeImageId).HasName("PK__ShoeImag__B3F93C0AA8E1E47B");

            entity.HasIndex(e => e.PublicId, "UQ__ShoeImag__87F1F39900515539").IsUnique();

            entity.HasIndex(e => e.Url, "UQ__ShoeImag__C5B214319B0ED74C").IsUnique();

            entity.Property(e => e.ShoeImageId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PublicId).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(500);

            entity.HasOne(d => d.Shoe).WithMany(p => p.ShoeImages)
                .HasForeignKey(d => d.ShoeId)
                .HasConstraintName("FK__ShoeImage__ShoeI__5FB337D6");
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.ShoppingCartId).HasName("PK__Shopping__7A789AE4A59BD0B8");

            entity.Property(e => e.ShoppingCartId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.DeliveryMethod).WithMany(p => p.ShoppingCarts)
                .HasForeignKey(d => d.DeliveryMethodId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ShoppingC__Deliv__787EE5A0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CDD483B79");

            entity.ToTable(tb => tb.HasTrigger("trg_Users_Audit"));

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053407DA5CD9").IsUnique();

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__UserRoles__RoleI__47DBAE45"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__UserRoles__UserI__46E78A0C"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760AD3CB4F4BA");
                        j.ToTable("UserRoles");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
