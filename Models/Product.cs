using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Models
{
    public class ProductLite
    {
        public int inv_mast_uid { get; set; }
        public string item_id { get; set; }
        public string item_desc { get; set; }
        public decimal qty_avail { get; set; }
        public string delete_flag { get; set; }
        public string class_id1 { get; set; }
        public string class_id2 { get; set; }
        public string class_id3 { get; set; }
        public string class_id4 { get; set; }
        public string class_id5 { get; set; }
        public decimal? weight { get; set; }
        public decimal? height { get; set; }
        public decimal? length { get; set; }
        public decimal? width { get; set; }
        public string product_type { get; set; }
        public string default_selling_unit { get; set; }
        public string display_on_web_flag { get; set; }
        public decimal price1 { get; set; }
        public decimal price2 { get; set; }
        public decimal price3 { get; set; }
        public decimal price4 { get; set; }
        public decimal price5 { get; set; }
        public decimal price6 { get; set; }
        public decimal price7 { get; set; }
        public decimal price8 { get; set; }
        public decimal price9 { get; set; }
        public decimal price10 { get; set; }

        public string class1_description { get; set; }
        public string class2_description { get; set; }
        public string class3_description { get; set; }
        public string class4_description { get; set; }
        public string class5_description { get; set; }

        public int total { get; set; }

        public decimal? unit_price { get; set; }
    }

    public class Product : ProductLite
    {
        public string extended_desc { get; set; }

        public IEnumerable<ProductUom> uoms { get; set; }
        public IEnumerable<ProductCategory> categories { get; set; }
    }

    public class ProductUom
    {
        public string unit_of_measure { get; set; }
        public string unit_description { get; set; }
        public decimal unit_size { get; set; }
        public string base_uom { get; set; }
        public string default_selling_unit { get; set; }
        public string default_pricing_unit { get; set; }
    }

    public class ProductCategory
    {
        public int item_category_uid { get; set; }
        public string item_category_id { get; set; }
        public string item_category_desc { get; set; }
    }
}
