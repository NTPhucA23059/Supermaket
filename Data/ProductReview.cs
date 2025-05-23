using System;
using System.Collections.Generic;

namespace Supermaket.Data;

public partial class ProductReview
{
    public int ReviewId { get; set; }

    public string? UserName { get; set; }

    public int? BillId { get; set; }

    public int Rating { get; set; }

    public string? ReviewText { get; set; }

    public DateTime ReviewDate { get; set; }

    public string? ProductName { get; set; }

    public virtual Bill? Bill { get; set; }
}
