using System;
using System.Collections.Generic;

namespace Supermaket.Data;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int? CartId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public string Status { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Product? Product { get; set; }
}
