using System;
using System.Collections.Generic;

namespace Supermaket.Data;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string PromotionCode { get; set; } = null!;

    public string? PromotionType { get; set; }

    public string? PromotionDescription { get; set; }

    public decimal? DiscountPercentage { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
