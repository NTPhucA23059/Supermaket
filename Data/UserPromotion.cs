using System;
using System.Collections.Generic;

namespace Supermaket.Data;

public partial class UserPromotion
{
    public int UserPromotionId { get; set; }

    public int? UserId { get; set; }

    public string? SubscriptionType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsSubscribed { get; set; }

    public virtual Account? User { get; set; }
}
