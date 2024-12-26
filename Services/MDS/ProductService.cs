using Dapper;
using Microsoft.Extensions.Logging;
using P21_latest_template.Models;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P21_latest_template.Services.MDS
{
    public class ProductService : Services.ProductService
    {
        public ProductService(IDbConnectionService dbConn, ILoggerFactory logger)
            : base(dbConn, logger)
        {
        }

        public override IEnumerable<ProductLite> FindByDateLastModified(ProductFilterParam param)
        {
            #region sql
            string sql = @"
;with items as
(
	select	m.inv_mast_uid
			,m.item_id
            ,m.item_desc
			,m.delete_flag                  
			,qty_avail =
			(
				select		sum(inv_loc.qty_on_hand) - 
							(
								sum(isnull(inv_loc.qty_allocated, 0)) 
								+ sum(isnull(inv_loc_stock_status.qty_non_pickable, 0))
								+ sum(isnull(inv_loc_stock_status.qty_quarantined, 0))
								+ sum(isnull(inv_loc_stock_status.qty_frozen, 0))
							)
				from		inv_loc 
							join location on inv_loc.location_id = location.location_id 
							join [address] on [address].id = inv_loc.location_id
							join inv_mast on inv_mast.inv_mast_uid = inv_loc.inv_mast_uid
							left join inv_loc_stock_status on inv_loc_stock_status.inv_mast_uid = inv_loc.inv_mast_uid
								and inv_loc_stock_status.location_id = inv_loc.location_id			
				where		isnull(inv_loc.delete_flag, 'N') <> 'Y'			
							and inv_mast.inv_mast_uid = m.inv_mast_uid
							and [location].delete_flag <> 'Y'
							and [location].location_type = 1330
							and inv_loc.company_id = @company_id

			)
			,m.class_id1
			,m.class_id2
			,m.class_id3
			,m.class_id4
			,m.class_id5
            ,m.[weight]
			,m.height
			,m.[length]
			,m.width	
            ,m.product_type
            ,m.default_selling_unit
            ,display_on_web_flag = case when
            (
                select	count(1)
                from	item_category_x_inv_mast ic
                where	ic.display_on_web_flag = 'Y'
		                and ic.delete_flag <> 'Y'
                        and ic.inv_mast_uid = m.inv_mast_uid
            ) > 0 then 'Y' else 'N' end
            ,m.price1
		    ,m.price2
		    ,m.price3
		    ,m.price4
		    ,m.price5
		    ,m.price6
		    ,m.price7
		    ,m.price8
		    ,m.price9
		    ,m.price10
			,class1_description = c1.class_description
			,class2_description = c2.class_description
			,class3_description = c3.class_description
			,class4_description = c4.class_description
			,class5_description = c5.class_description
            ,row_num = row_number() over (order by m.inv_mast_uid)			
	from	inv_mast m			
			left join class c1 on m.class_id1 = c1.class_id
				and c1.class_number = 1
				and c1.class_type = 'IV'
			left join class c2 on m.class_id2 = c2.class_id
				and c2.class_number = 2
				and c2.class_type = 'IV'	
			left join class c3 on m.class_id3 = c3.class_id
				and c3.class_number = 3
				and c3.class_type = 'IV'
			left join class c4 on m.class_id4 = c4.class_id
				and c4.class_number = 4
				and c4.class_type = 'IV'
			left join class c5 on m.class_id5 = c5.class_id
				and c5.class_number = 5
				and c5.class_type = 'IV'	
	where	
			(
				m.date_last_modified >= @date_last_modified						
				or m.inv_mast_uid in
				(
					select	il.inv_mast_uid
					from	inv_loc il
							join [location] l on il.location_id = l.location_id
								and il.company_id = l.company_id
								and l.location_type = 1330									
					where	il.company_id = @company_id
							and
							(
                                il.date_last_modified >= @date_last_modified						
                                or il.inv_last_changed_date >= @date_last_modified
                            )
					group by il.inv_mast_uid
				)						
			)
            and case when len(@product_type) > 0 then @product_type else m.product_type end = m.product_type
            {{REPLACE}}
)
select		items.*
			,Total = cnt
from		items
			cross join (select cnt = count(1) from items) as cnt
--where		row_num between (@page_size * (@page_number - 1)) + 1 and (@page_number * @page_size)
order by	items.inv_mast_uid
offset @page_size * (@page_number - 1) rows
fetch next @page_size rows only;
";
            #endregion

            try
            {
                if (!string.IsNullOrEmpty(param.class_id) && param.class_values != null && param.class_values.Any())
                {
                    string class_id = "";
                    switch (param.class_id)
                    {
                        case "class_id1":
                            class_id = "class_id1";
                            break;
                        case "class_id2":
                            class_id = "class_id2";
                            break;
                        case "class_id3":
                            class_id = "class_id3";
                            break;
                        case "class_id4":
                            class_id = "class_id4";
                            break;
                        case "class_id5":
                            class_id = "class_id5";
                            break;
                    }

                    if (!string.IsNullOrEmpty(class_id))
                        sql = sql.Replace("{{REPLACE}}", $"and {class_id} in @class_values");
                    else
                        sql = sql.Replace("{{REPLACE}}", "");
                }
                else
                {
                    sql = sql.Replace("{{REPLACE}}", "");
                }

                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();

                    object p = new
                    {
                        param.page_number,
                        param.page_size,
                        param.company_id,
                        param.date_last_modified,
                        product_type = param.product_type ?? ""
                    };

                    if (sql.Contains("@class_values"))
                    {
                        p = new
                        {
                            param.page_number,
                            param.page_size,
                            param.company_id,
                            param.date_last_modified,
                            product_type = param.product_type ?? "",
                            param.class_id,
                            param.class_values
                        };
                    }

                    return conn.Query<ProductLite>(sql, p).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString(), param);
            }
            return null;
        }
    }
}
