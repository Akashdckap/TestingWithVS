using P21_latest_template.Entities;
using P21_latest_template.Models;

namespace P21_latest_template.Helpers
{
    public static class AddressHelper
    {
        public static void SetDefaultValues(this Address address,
            AddressCreate model, CustDefault custDefault)
        {
            address.mail_address1 = model.mail_address1;
            address.mail_address2 = model.mail_address2;
            address.mail_city = model.mail_city;
            address.mail_state = model.mail_state;
            address.mail_postal_code = model.mail_postal_code;
            address.mail_country = model.mail_country;

            address.phys_address1 = model.phys_address1;
            address.phys_address2 = model.phys_address2;
            address.phys_city = model.phys_city;
            address.phys_state = model.phys_state;
            address.phys_postal_code = model.phys_postal_code;
            address.phys_country = model.phys_country;

            address.central_phone_number = model.central_phone_number;
            address.invoice_type = custDefault.invoice_type;
            address.inventory_location_flag = "N";
        }
    }
}
