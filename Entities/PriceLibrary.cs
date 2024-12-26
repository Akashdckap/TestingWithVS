using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("price_library")]
    public class PriceLibrary
    {
        [ExplicitKey]
        public int price_library_uid { get; set; }
        public string price_library_id { get; set; }
        public string description { get; set; }
        public int category_cd { get; set; }
        public int type_cd { get; set; }
        public int? source_price_cd { get; set; }
        public decimal? multiplier { get; set; }
        public int row_status_flag { get; set; }
        public DateTime date_last_modified { get; set; }
        public DateTime date_created { get; set; }
        public string last_maintained_by { get; set; }
        public string created_by { get; set; }
        public decimal? terminal_id { get; set; }
        public string company_id { get; set; }
        public string strategic_price_library_flag { get; set; }
        public int? customer_size_cd { get; set; }
        public int? customer_category_uid { get; set; }
        public string library_on_contract { get; set; }
    }
}
