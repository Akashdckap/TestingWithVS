using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("corp_id")]
    public class CorpId
    {
        [ExplicitKey]
        public string company_id { get; set; }
        [ExplicitKey]
        public decimal address_id { get; set; }
        public decimal credit_limit { get; set; } = 0;
        public decimal credit_limit_used { get; set; } = 0;
        public string delete_flag { get; set; } = "N";
        public DateTime date_created { get; set; } = DateTime.Now;
        public DateTime date_last_modified { get; set; } = DateTime.Now;
        public string last_maintained_by { get; set; } = "admin";
        public string address_name { get; set; }
    }
}
