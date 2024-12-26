namespace P21_latest_template.Models
{
    public class ProductPrice
    {
        public int inv_mast_uid { get; set; }
        public string item_id { get; set; }
        public decimal unit_price { get; set; }
        public string uom { get; set; }
        public int? price_page_uid { get; set; }
    }
}
