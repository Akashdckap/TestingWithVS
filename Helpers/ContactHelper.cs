using P21_latest_template.Entities;
using P21_latest_template.Models;

namespace P21_latest_template.Helpers
{
    public static class ContactHelper
    {
        public static void SetDefaultValues(this Contact contact,
            ContactCreate model, decimal addressId)
        {
            contact.first_name = model.first_name;
            contact.last_name = model.last_name;
            contact.title = model.title;
            contact.direct_phone = model.direct_phone;
            contact.email_address = model.email_address;
            contact.address_id = addressId;
        }
    }
}
