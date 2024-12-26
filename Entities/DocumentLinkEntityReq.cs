using Dapper.Contrib.Extensions;
using System;

namespace P21_latest_template.Entities
{
    [Table("document_link_entity_req")]
    public class DocumentLinkEntityReq
    {
        [ExplicitKey]
        public int document_link_entity_req_uid { get; set; }
        public string company_id { get; set; }
        public int entity_id { get; set; }
        public int entity_type { get; set; } = 1307;
        public string require_lot_documentation_flag { get; set; } = "N";
        public string print_flag { get; set; } = "N";
        public string fax_flag { get; set; } = "N";
        public string email_flag { get; set; } = "N";
        public int number_of_copies { get; set; } = 0;
        public int row_status_flag { get; set; } = 704;
        public DateTime date_created { get; set; } = DateTime.Now;
        public string created_by { get; set; } = "admin";
        public DateTime date_last_modified { get; set; } = DateTime.Now;
        public string last_maintained_by { get; set; } = "admin";
        public string send_outside_use_docs_flag { get; set; } = "N";
        public string send_outside_use_print_flag { get; set; } = "N";
        public string send_outside_use_fax_flag { get; set; } = "N";
        public string send_outside_use_email_flag { get; set; } = "N";
        public int outside_use_number_of_copies { get; set; } = 0;
    }
}
