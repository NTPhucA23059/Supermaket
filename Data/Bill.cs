using System;
using System.Collections.Generic;

namespace Supermaket.Data;

public partial class Bill
{
    public int BillId { get; set; }

    public int? OrderId { get; set; }

    public string? UserName { get; set; }

    public DateTime BillDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public string? TransactionId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
