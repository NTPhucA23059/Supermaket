using System;
using System.Collections.Generic;

namespace Supermaket.Data;

public partial class AdImage
{
    public int AdId { get; set; }

    public string AdTitle { get; set; } = null!;

    public string? AdDescription { get; set; }

    public string AdImageUrl { get; set; } = null!;

    public string? AdLink { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
