using System.Collections.Generic;

namespace P21_latest_template.Models
{
    public class ProductAvailability
    {
        public IEnumerable<ProductInventory> ItemAvailability { get; set; }
    }

    public class ProductInventory
    {
        public int inv_mast_uid { get; set; }
        public string ItemId { get; set; }
        public string CompanyId { get; set; }
        public decimal LocationId { get; set; }
        public decimal QuantityAvailable { get; set; }
    }
}
