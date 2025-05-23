using System;
using System.Collections.Generic;

namespace Supermaket.Data;

public partial class OrderPromotion
{
    public int OrderPromotionId { get; set; }

    public int? OrderId { get; set; }

    public int? PromotionId { get; set; }

    public string? DiscountAmountApplied { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Promotion? Promotion { get; set; }
}
