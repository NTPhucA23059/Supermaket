using AutoMapper;

using Supermaket.Areas.Admin.Models;
using Supermaket.Data;

namespace Supermaket.Helpers
{
    public class AutoMapperAdmin : Profile
    {
        public AutoMapperAdmin()
        {

            CreateMap<Product, ProductModels>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.ProductImage))
                .ForMember(dest => dest.AdditionalImages, opt => opt.MapFrom(src => src.AdditionalImages))
               .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount));


            CreateMap<ProductModels, Product>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));


            CreateMap<Brand, BrandModels>()
               .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
               .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            // Mapping from BrandModels to Brand (for adding/updating brand)
            CreateMap<BrandModels, Brand>()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Brand, BrandModels>()
               .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
               .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));


            CreateMap<BrandModels, Brand>()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));


            CreateMap<Category, CategoryModels>()
              .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
              .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
              .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<CategoryModels, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Promotion, PromotionModels>()
               .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
               .ForMember(dest => dest.PromotionCode, opt => opt.MapFrom(src => src.PromotionCode))
               .ForMember(dest => dest.PromotionType, opt => opt.MapFrom(src => src.PromotionType))
               .ForMember(dest => dest.PromotionDescription, opt => opt.MapFrom(src => src.PromotionDescription))
               .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentage))
               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
               .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<PromotionModels, Promotion>()
                .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
                .ForMember(dest => dest.PromotionCode, opt => opt.MapFrom(src => src.PromotionCode))
                .ForMember(dest => dest.PromotionType, opt => opt.MapFrom(src => src.PromotionType))
                .ForMember(dest => dest.PromotionDescription, opt => opt.MapFrom(src => src.PromotionDescription))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.DiscountPercentage))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));


            CreateMap<Account, AccountModels>()
               .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
               .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.ContactNumber, opt => opt.MapFrom(src => src.ContactNumber))
               .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
               .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
               .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
               .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
               .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
               .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
               .ForMember(dest => dest.CreditCardNumber, opt => opt.MapFrom(src => src.CreditCardNumber))
               .ForMember(dest => dest.CreditCardExpiry, opt => opt.MapFrom(src => src.CreditCardExpiry));

            CreateMap<AccountModels, Account>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ContactNumber, opt => opt.MapFrom(src => src.ContactNumber))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))  // Assigning current time for CreatedAt
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))  // Assigning current time for UpdatedAt
                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
                .ForMember(dest => dest.CreditCardNumber, opt => opt.MapFrom(src => src.CreditCardNumber))
                .ForMember(dest => dest.CreditCardExpiry, opt => opt.MapFrom(src => src.CreditCardExpiry));


            CreateMap<Order, OrderModels>()
               .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
               .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
               .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
               .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
               .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
               .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
               .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
               .ForMember(dest => dest.Bills, opt => opt.MapFrom(src => src.Bills))
               .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
               .ForMember(dest => dest.Promotion, opt => opt.MapFrom(src => src.Promotion))
               .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            // Mapping from OrderModels to Order
            CreateMap<OrderModels, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.PromotionId, opt => opt.MapFrom(src => src.PromotionId))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
                .ForMember(dest => dest.Bills, opt => opt.MapFrom(src => src.Bills))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Promotion, opt => opt.MapFrom(src => src.Promotion))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            // Mapping từ OrderItem sang OrderItemModels
            CreateMap<OrderItem, OrderItemModels>()
                .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.OrderItemId))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                 .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));

            // Mapping từ OrderItemModels sang OrderItem (cho việc thêm/sửa OrderItem)
            CreateMap<OrderItemModels, OrderItem>()
                .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.OrderItemId))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                 .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
            // Mapping from Bill to BillModels
            CreateMap<Bill, BillModels>()
                .ForMember(dest => dest.BillId, opt => opt.MapFrom(src => src.BillId))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.BillDate, opt => opt.MapFrom(src => src.BillDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.ProductReviews, opt => opt.MapFrom(src => src.ProductReviews));

            // Mapping from BillModels to Bill
            CreateMap<BillModels, Bill>()
                .ForMember(dest => dest.BillId, opt => opt.MapFrom(src => src.BillId))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.BillDate, opt => opt.MapFrom(src => src.BillDate))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order))
                .ForMember(dest => dest.ProductReviews, opt => opt.MapFrom(src => src.ProductReviews));



        }
    }
}



