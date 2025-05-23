using System;
using System.Collections.Generic;

namespace Supermaket.Data;

public partial class UserProductHistory
{
    public int HistoryId { get; set; }

    public int? UserId { get; set; }

    public string? ActionType { get; set; }

    public DateTime ActionDate { get; set; }

    public string Status { get; set; } = null!;

    public string? IpAddress { get; set; }

    public virtual Account? User { get; set; }
}
