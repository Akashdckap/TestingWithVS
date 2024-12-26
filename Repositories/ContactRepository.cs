using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Logging;
using P21_latest_template.Entities;
using P21_latest_template.Repositories;
using P21_latest_template.Services;
using Serilog.Core;
using System;

namespace P21_latest_template.Repositories
{
    public interface IContactRepository : IRepository<Contact, string>
    {

    }

    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public Contact Get(string id)
        {
            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    return conn.Get<Contact>(id);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, id);
            }
            return null;
        }

        public override bool Create(Contact entity)
        {
            entity.date_created = DateTime.Now;
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Create(entity);
        }

        public override bool Update(Contact entity)
        {
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Update(entity);
        }
    }
}
