using Microsoft.EntityFrameworkCore;
using MiFloraBack.Models;

namespace MiFloraBack.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Существующие сущности
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserShop> UserShops { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<Spoilage> Spoilages { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }
        public DbSet<DeliveryStatus> DeliveryStatuses { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Loyalty> Loyalties { get; set; }
        public DbSet<LoyaltyTransaction> LoyaltyTransactions { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<CorporateClient> CorporateClients { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

        // Новые сущности для "Склада"
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Категории с подкатегориями
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Subcategories)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Курьеры в статусе доставки
            modelBuilder.Entity<DeliveryStatus>()
                .HasOne(d => d.Courier)
                .WithMany(u => u.DeliveryStatusesAsCourier)
                .HasForeignKey(d => d.CourierId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Уникальность ролей пользователя по контексту
            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => new { ur.UserId, ur.RoleId, ur.BusinessId, ur.BranchId, ur.ShopId })
                .IsUnique();

            // --- Конфигурация "Склада"

            // Contractor → Invoices (1:N)
            modelBuilder.Entity<Contractor>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Contractor)
                .HasForeignKey(i => i.ContractorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Branch → Invoices (1:N)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Branch)
                .WithMany()
                .HasForeignKey(i => i.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Invoice → InvoiceItems (1:N)
            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Invoice)
                .WithMany(i => i.Items)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product → InvoiceItems (1:N)
            modelBuilder.Entity<InvoiceItem>()
                .HasOne(ii => ii.Product)
                .WithMany(p => p.InvoiceItems)    // теперь совпадает с новым свойством
                .HasForeignKey(ii => ii.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
