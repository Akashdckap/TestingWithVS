using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using P21_latest_template.Services;
using P21_latest_template.Entities;
using System;

namespace P21_latest_template.Repositories
{
    public interface IAddressRepository : IRepository<Address, decimal>
    {

    }

    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public override bool Create(Address entity)
        {
            entity.date_created = DateTime.Now;
            entity.date_last_modified = DateTime.Now;
            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";
            entity.address_id_string = entity.id.ToString("G29");
            return base.Create(entity);
        }

        public override bool Update(Address entity)
        {
            entity.date_last_modified = DateTime.Now;
            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";
            return base.Update(entity);
        }

        public virtual Address Get(decimal id)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    return conn.Get<Address>(id);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, id);
            }
            return null;
        }
    }
}
