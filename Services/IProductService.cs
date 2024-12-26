using System;
using System.Collections.Generic;
using P21_latest_template.Models;

namespace P21_latest_template.Services
{
    public interface IProductService
    {
        IEnumerable<ProductLite> FindByDateLastModified(ProductFilterParam param);
        Product Get(int inv_mast_uid);
        ProductFound GetProductFound(int inv_mast_uid,
            string item_id, string uom, decimal location_id);
        ProductPrice GetProductPrice(string company_id, decimal customer_id,
            ProductFound product, decimal qty);
        IEnumerable<ProductInventory> GetInventories(IEnumerable<decimal> location_ids,
            DateTime date_last_modified, string product_type = "");
    }
}