using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P21_latest_template.Models
{
    public class CustomerPriceParam
    {
        [Required]
        public string company_id { get; set; }
        [Required]
        public decimal customer_id { get; set; }
        [Required]
        public decimal location_id { get; set; }
        [Required]
        public IEnumerable<ProductPriceParam> Products { get; set; } = new List<ProductPriceParam>();
    }

    public class ProductPriceParam
    {
        public int? inv_mast_uid { get; set; }
        public string item_id { get; set; }
        [Required]
        public string uom { get; set; }
        [Required]
        public decimal qty { get; set; }
    }
}
