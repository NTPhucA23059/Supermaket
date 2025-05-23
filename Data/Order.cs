using System;
using System.Collections.Generic;

namespace Supermaket.Data;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public int? PromotionId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string ShippingAddress { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public decimal? DiscountAmount { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Promotion? Promotion { get; set; }

    public virtual Account? User { get; set; }
}
