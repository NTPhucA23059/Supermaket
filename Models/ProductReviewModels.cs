using Supermaket.Data;

namespace Supermaket.Models
{
    public class ProductReviewModels
    {

        public int ReviewId { get; set; }

        public int? BillId { get; set; }

        public int Rating { get; set; }

        public string? ReviewText { get; set; }

        public DateTime ReviewDate { get; set; }

        public virtual Bill? Bill { get; set; }


        public string userName { get; set; }

        public string? ProductName { get; set; }
        public int ProductId { get; set; }

	}
}
