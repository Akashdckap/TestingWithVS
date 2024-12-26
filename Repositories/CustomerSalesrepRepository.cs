using Microsoft.Extensions.Logging;
using P21_latest_template.Repositories;
using P21_latest_template.Services;
using P21_latest_template.Entities;
using System;

namespace P21_latest_template.Repositories
{
    public interface ICustomerSalesrepRepository : IRepository<CustomerSalesrep, int>
    {

    }

    public class CustomerSalesrepRepository : GenericRepository<CustomerSalesrep>, ICustomerSalesrepRepository
    {
        public CustomerSalesrepRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public override bool Create(CustomerSalesrep entity)
        {
            entity.date_created = DateTime.Now;
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.created_by))
                entity.created_by = "admin";
            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Create(entity);
        }

        public override bool Update(CustomerSalesrep entity)
        {
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Update(entity);
        }
    }
}
