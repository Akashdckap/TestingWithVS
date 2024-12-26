using P21_latest_template.Models;
using System.Collections.Generic;

namespace P21_latest_template.Services
{
    public interface ICustomerService
    {
        CustomerCreate Create(CustomerCreate model);
        IEnumerable<Models.MDS.CustomerLite> Find(CustomerFilterParam param);
        Models.MDS.Customer GetCustomer(string company_id, decimal customer_id, int total);
        CustomerContactCreate CreateContact(CustomerContactCreate model);
        CustomerCreate GetCustomerByContactEmail(string email_address, string name, string mail_state, string mail_postal_code, string phys_state, string phys_postal_code, ContactCreate contact);
        CustomerCreate GetCustomerByp21ID(decimal customer_id, string company_name, ContactCreate contact);
    }
}