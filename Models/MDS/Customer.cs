using System.Collections.Generic;

namespace P21_latest_template.Models.MDS
{
    public class CustomerLite
    {
        public string salesrep_id { get; set; }
        public string company_id { get; set; }
        public decimal customer_id { get; set; }
        public string customer_name { get; set; }
        public string delete_flag { get; set; }
        public string po_no_required { get; set; }
        public decimal credit_limit { get; set; }
        public decimal credit_limit_used { get; set; }
        public decimal credit_limit_per_order { get; set; }
        public string credit_status { get; set; }
        public int Total { get; set; }
    }

    public class Customer : CustomerLite
    {
        public IEnumerable<CustomerContact> Contacts { get; set; } = new List<CustomerContact>();
        public IEnumerable<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
    }

    public class CustomerContact : ContactCreate
    {
        public string delete_flag { get; set; }
    }

    public class CustomerAddress : AddressCreate
    {
        public string delete_flag { get; set; }
    }
}
