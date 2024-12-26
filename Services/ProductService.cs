using Dapper;
using Microsoft.Extensions.Logging;
using P21_latest_template.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P21_latest_template.Services
{
    public class ProductService : IProductService
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger<ProductService> Logger;

        public ProductService(
            IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
        {
            DbConn = dbConn;
            Logger = loggerFactory.CreateLogger<ProductService>();
        }

        public virtual IEnumerable<ProductLite> FindByDateLastModified(ProductFilterParam param)
        {
            //CTE common table expression with items as means
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
			--case @class_id 
			--	when 'class_id1' then m.class_id1
			--	when 'class_id2' then m.class_id2
			--	when 'class_id3' then m.class_id3
			--	when 'class_id4' then m.class_id4
			--	when 'class_id5' then m.class_id5
			--	else ''
			--end = case when len(@class_value) > 0 then @class_value else '' end 
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

        public virtual Product Get(int inv_mast_uid)
        {
            #region sql
            string sql = @"
select	m.inv_mast_uid
		,item_id
		,delete_flag
		,item_desc		
		,extended_desc
		,[weight] = isnull(weight, 0)	
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
from	inv_mast m		
where	m.inv_mast_uid = @inv_mast_uid
		
-- uom
select	iu.unit_of_measure
		,uom.unit_description
		,iu.unit_size
		,base_uom = case when m.base_unit = iu.unit_of_measure then 'Y' else 'N' end
		,default_selling_unit = case when m.default_selling_unit = iu.unit_of_measure then 'Y' else 'N' end
		,default_pricing_unit = case when m.sales_pricing_unit = iu.unit_of_measure then 'Y' else 'N' end				
from	inv_mast m
		join item_uom iu on m.inv_mast_uid = iu.inv_mast_uid
		join unit_of_measure uom on iu.unit_of_measure = uom.unit_id	
where	uom.delete_flag <> 'Y'		
		and m.inv_mast_uid = @inv_mast_uid

-- categories
select	item_category.item_category_uid
		,item_category.item_category_id        		
		,item_category.item_category_desc
from	item_category_x_inv_mast						
        join item_category on item_category.item_category_uid = item_category_x_inv_mast.item_category_uid	
where	item_category_x_inv_mast.inv_mast_uid = @inv_mast_uid
		and item_category_x_inv_mast.delete_flag <> 'Y'
		and item_category.delete_flag <> 'Y'";
            #endregion

            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();
                    using (var multi = conn.QueryMultiple(sql, new { inv_mast_uid }))
                    {
                        var item = multi.Read<Product>().SingleOrDefault();
                        if (item == null) return null;
                        item.uoms = multi.Read<ProductUom>().ToList();
                        item.categories = multi.Read<ProductCategory>().ToList();

                        return item;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString(), inv_mast_uid);
            }
            return null;
        }

        public virtual ProductFound GetProductFound(int inv_mast_uid,
            string item_id, string uom, decimal location_id)
        {
            string sql = @"
select	m.inv_mast_uid
		,m.item_id
		,u.unit_of_measure
		,u.unit_size
		,l.location_id
		,delete_flag = case when m.delete_flag = 'Y' or isnull(l.delete_flag, 'N') = 'Y' then 'Y' else 'N' end
		,l.discontinued
from	inv_mast m
		join item_uom u on m.inv_mast_uid = u.inv_mast_uid
		join inv_loc l on m.inv_mast_uid = l.inv_mast_uid
where	--case when @inv_mast_uid > 0 then m.inv_mast_uid else @inv_mast_uid end = @inv_mast_uid
		--and 
        case when len(@item_id) > 0 then m.item_id else @item_id end = @item_id
		and u.unit_of_measure = @uom
		and l.location_id = @location_id
";
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();
                    return conn.Query<ProductFound>(sql, new
                    {
                        inv_mast_uid,
                        item_id,
                        uom,
                        location_id
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString(), inv_mast_uid);
            }
            return null;
        }

        public virtual ProductPrice GetProductPrice(string company_id, decimal customer_id,
            ProductFound product, decimal qty)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    conn.Open();
                    //DynamicParameters is from Dapper
                    var sqlParam = new DynamicParameters();
                    sqlParam.Add("  ", customer_id);
                    sqlParam.Add("@company_id", company_id);
                    sqlParam.Add("@inv_mast_uid", product.inv_mast_uid);
                    sqlParam.Add("@supplier_id", null);
                    sqlParam.Add("@disc_group_id", null);
                    sqlParam.Add("@prod_group_id", null);
                    sqlParam.Add("@mfr_class_id", null);
                    sqlParam.Add("@customer_part_no", product.item_id);
                    sqlParam.Add("@tran_date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    sqlParam.Add("@oe_sales_unit_size", product.unit_size);
                    sqlParam.Add("@oe_qty_ordered", qty);
                    sqlParam.Add("@source_location_id", product.location_id);
                    sqlParam.Add("@oe_pricing_unit_size", product.unit_size);
                    sqlParam.Add("@sales_cost", null);
                    sqlParam.Add("@debug_mode", 0);
                    sqlParam.Add("@summary_price", 1);
                    sqlParam.Add("@order_type", 0);
                    sqlParam.Add("@rollup_component_price", "N");
                    sqlParam.Add("@calculator_type", "B");
                    sqlParam.Add("@udl_list", null);
                    sqlParam.Add("@configuration_id", 173);
                    sqlParam.Add("@oe_source_location_id", product.location_id);
                    sqlParam.Add("@limit_by_location_id", "N");
                    sqlParam.Add("@forced_price_value", null);
                    sqlParam.Add("@check_inventory", "N");
                    sqlParam.Add("@use_web_based_pricing", "N");
                    sqlParam.Add("@sales_location_id", product.location_id);
                    sqlParam.Add("@ship_to_id", null);
                    sqlParam.Add("@base_price_library_uid", null);
                    sqlParam.Add("@selected_price_library_uid", null);
                    sqlParam.Add("@customer_sensitivity_value", null);
                    sqlParam.Add("@customer_category_uid", null);
                    sqlParam.Add("@data_service_level", null);
                    sqlParam.Add("@data_services_exp_date_is_valid", null);
                    sqlParam.Add("@audit", "Y");
                    sqlParam.Add("@carrier_contract_line_uid", null);
                    sqlParam.Add("@carrier_calc_type", null);
                    sqlParam.Add("@carrier_forced_price", null);
                    sqlParam.Add("@future_price_date", null);
                    sqlParam.Add("@use_distributor_net_library", "N");
                    sqlParam.Add("@return_all_revisions", "N");
                    sqlParam.Add("@arg_item_revision_level_list", "");
                    sqlParam.Add("@rolled_item_pricing_type_cd", "3548");

                    var data = conn.Query<dynamic>("p21_price_engine", sqlParam,
                        commandType: System.Data.CommandType.StoredProcedure)
                        .FirstOrDefault();

                    if (data != null)
                    {
                        return new ProductPrice()
                        {
                            inv_mast_uid = product.inv_mast_uid,
                            item_id = product.item_id,
                            uom = product.unit_of_measure,
                            unit_price = data.unit_price,
                            price_page_uid = data.price_page_uid
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
            return null;
        }

        public virtual IEnumerable<ProductInventory> GetInventories(IEnumerable<decimal> location_ids,
            DateTime date_last_modified, string product_type = "")
        {
            #region sql
            string sql = @"
select	inv_mast.inv_mast_uid
		,ItemId = inv_mast.item_id
		,CompanyId = inv_loc.company_id
		,LocationId = inv_loc.location_id
		,QuantityAvailable = inv_loc.qty_on_hand - 
		(
			isnull(inv_loc.qty_allocated, 0) 
			+ isnull(inv_loc_stock_status.qty_non_pickable, 0)
			+ isnull(inv_loc_stock_status.qty_quarantined, 0)
			+ isnull(inv_loc_stock_status.qty_frozen, 0)
		)
from	inv_loc 
		join [location] on inv_loc.location_id = [location].location_id
		join company on inv_loc.company_id = company.company_id
		join inv_mast on inv_mast.inv_mast_uid = inv_loc.inv_mast_uid
		left join inv_loc_stock_status on inv_loc_stock_status.inv_mast_uid = inv_loc.inv_mast_uid
			and inv_loc_stock_status.location_id = inv_loc.location_id			
where	isnull(inv_loc.delete_flag, 'N') <> 'Y'			
		and [location].delete_flag <> 'Y'
		and [location].location_type = 1330
		and company.delete_flag <> 'Y'
		and inv_loc.location_id in @location_ids
		and inv_loc.inv_mast_uid in
		(
			select	inv_mast_uid
			from	inv_loc l
			where	isnull(l.delete_flag, 'N') <> 'Y' 
					and 
					(
						l.inv_last_changed_date >= @date_last_modified
						or l.date_last_modified >= @date_last_modified
					)
			group by l.inv_mast_uid
		)
        and case when len(@product_type) > 0 then @product_type else inv_mast.product_type end = inv_mast.product_type
order by inv_mast.item_id
		,inv_loc.location_id
";
            #endregion

            using (var conn = DbConn.GetP21DbConnection())
            {
                try
                {
                    conn.Open();
                    return conn.Query<ProductInventory>(sql, new
                    {
                        location_ids,
                        date_last_modified,
                        product_type
                    }).ToList();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message, location_ids, date_last_modified);
                }
                finally
                {
                    conn.Close();
                }
            }
            return null;
        }
    }
}
