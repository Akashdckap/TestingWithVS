using P21_latest_template.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Helpers
{
    public static class ShipToHelper
    {
        public static void SetDefaultValues(this ShipTo shipto,
            Customer customer, CustDefault custDefault, Company company)
        {
            shipto.acceptable_wait_time = customer.acceptable_wait_time;
            shipto.preferred_location_id = custDefault.preferred_location_id;
            shipto.default_branch = customer.default_branch_id;
            shipto.packing_basis = custDefault.packing_basis;
            shipto.accept_partial_orders = customer.accept_partial_orders;
            shipto.terms_id = customer.terms_id;
            shipto.invoice_type = custDefault.invoice_type;
            shipto.billed_on_gross_net_qty = customer.billed_on_gross_net_qty;
            shipto.pick_ticket_type = customer.pick_ticket_type;
            shipto.include_non_alloc_on_pick_tix = customer.include_non_alloc_on_pick_tix;
            shipto.include_non_alloc_on_pack_list = customer.include_non_alloc_on_pack_list;
            shipto.freight_code_uid = company.freight_code_uid;
            shipto.signature_required = customer.signature_required_flag;
            shipto.exclude_canceld_from_pick_tix = customer.exclude_canceld_from_pick_tix;
            shipto.exclude_canceld_from_pack_list = customer.exclude_canceld_from_pack_list;
            shipto.print_packinglist_in_shipping = customer.print_packinglist_in_shipping;
            shipto.print_prices_on_packinglist = customer.print_prices_on_packinglist;
            shipto.degree_days_flag = "N";
            shipto.calendar_days_flag = "N";
            //shipto.heat_and_hot_water_cd = 1;
            shipto.taxable_flag = customer.taxable_flag;
            shipto.small_truck_flag = "N";
            shipto.preferred_location_id = custDefault.preferred_location_id;
        }
    }
}
