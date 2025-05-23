using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Supermaket.Data;

public partial class OnlineSupermarketContext : DbContext
{
    public OnlineSupermarketContext()
    {
    }

    public OnlineSupermarketContext(DbContextOptions<OnlineSupermarketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.;Database=OnlineSupermarket;Persist Security Info=True;User ID=sa;Password=sa;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__F267251ECA299847");

            entity.HasIndex(e => e.ContactNumber, "UQ__Accounts__4F86E9D754FD6BB4").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Accounts__AB6E6164E128BE89").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Accounts__F3DBC57276FB2BD3").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("accountId");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .HasColumnName("contactNumber");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreditCardExpiry)
                .HasMaxLength(5)
                .HasColumnName("creditCardExpiry");
            entity.Property(e => e.CreditCardNumber)
                .HasMaxLength(19)
                .HasColumnName("creditCardNumber");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.LastLogin)
                .HasColumnType("datetime")
                .HasColumnName("lastLogin");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(255)
                .HasColumnName("profilePicture");
            entity.Property(e => e.RandomKey)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ResetPasswordCode).HasMaxLength(50);
            entity.Property(e => e.ResetPasswordExpires).HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bill__6D903F036B61AF4A");

            entity.ToTable("Bill");

            entity.Property(e => e.BillId).HasColumnName("billId");
            entity.Property(e => e.BillDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("billDate");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("paymentMethod");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("paymentStatus");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalAmount");
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .HasColumnName("transactionId");
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .HasColumnName("userName");

            entity.HasOne(d => d.Order).WithMany(p => p.Bills)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Bill_Orders");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__06B77299E1CADB74");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandId).HasColumnName("brandId");
            entity.Property(e => e.BrandName)
                .HasMaxLength(100)
                .HasColumnName("brandName");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("PK__Cart__415B03B8E20D65F8");

            entity.ToTable("Cart");

            entity.Property(e => e.CartId).HasColumnName("cartId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK__CartItem__283983B66C161E1A");

            entity.ToTable("CartItem");

            entity.Property(e => e.CartItemId).HasColumnName("cartItemId");
            entity.Property(e => e.CartId).HasColumnName("cartId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_CartItem_Cart");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_CartItem_Products");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__23CAF1D830533197");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("categoryName");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__0809335D679A1F3A");

            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.DiscountAmount)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discountAmount");
            entity.Property(e => e.FullName)
                .HasMaxLength(30)
                .HasColumnName("fullName");
            entity.Property(e => e.Note)
                .HasMaxLength(500)
                .HasColumnName("note");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("orderDate");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .HasColumnName("phone");
            entity.Property(e => e.PromotionId).HasColumnName("promotionId");
            entity.Property(e => e.ShippingAddress)
                .HasMaxLength(500)
                .HasColumnName("shippingAddress");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalAmount");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_Orders_Promotions");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Orders_Accounts");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__3724BD52078F907D");

            entity.Property(e => e.OrderItemId).HasColumnName("orderItemId");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending")
                .HasColumnName("status");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderItems_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__2D10D16A31843DFC");

            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.AdditionalImages).HasColumnName("additionalImages");
            entity.Property(e => e.BrandId).HasColumnName("brandId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("discount");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ProductImage)
                .HasMaxLength(255)
                .HasColumnName("productImage");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("productName");
            entity.Property(e => e.QuantityInStock).HasColumnName("quantityInStock");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.ViewCount).HasColumnName("viewCount");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_Products_Brand");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Products_Category");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__ProductR__2ECD6E0467C2B7C7");

            entity.Property(e => e.ReviewId).HasColumnName("reviewId");
            entity.Property(e => e.BillId).HasColumnName("billId");
            entity.Property(e => e.ProductName)
                .HasMaxLength(255)
                .HasColumnName("productName");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReviewDate)
                .HasColumnType("datetime")
                .HasColumnName("reviewDate");
            entity.Property(e => e.ReviewText)
                .HasMaxLength(1000)
                .HasColumnName("reviewText");
            entity.Property(e => e.UserName)
                .HasMaxLength(30)
                .HasColumnName("userName");

            entity.HasOne(d => d.Bill).WithMany(p => p.ProductReviews)
                .HasForeignKey(d => d.BillId)
                .HasConstraintName("FK_ProductReviews_Bill");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__99EB696EF358A0A4");

            entity.Property(e => e.PromotionId).HasColumnName("promotionId");
            entity.Property(e => e.DiscountPercentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discountPercentage");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.PromotionCode)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("promotionCode");
            entity.Property(e => e.PromotionDescription)
                .HasMaxLength(500)
                .HasColumnName("promotionDescription");
            entity.Property(e => e.PromotionType)
                .HasMaxLength(50)
                .HasColumnName("promotionType");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Active")
                .HasColumnName("status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
