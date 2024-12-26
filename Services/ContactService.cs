using Dapper;
using Microsoft.Extensions.Logging;
using P21_latest_template.Repositories;
using P21_latest_template.Services;
using P21_latest_template.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P21_latest_template.Services
{
    public class ContactService : IContactService
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger Logger;
        protected readonly IContactRepository ContactRepository;

        public ContactService(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory,
            IContactRepository contactRepo)
        {
            DbConn = dbConn;
            Logger = loggerFactory.CreateLogger<ContactService>();
            ContactRepository = contactRepo;
        }

        public ContactCreate Update(ContactCreate model)
        {
            var entity = ContactRepository.Get(model.id);

            if (entity == null)
                return null;

            entity.first_name = model.first_name;
            entity.last_name = model.last_name;
            entity.title = model.title;
            entity.direct_phone = model.direct_phone;
            entity.email_address = model.email_address;
            entity.date_last_modified = DateTime.Now;
            entity.last_maintained_by = "admin";

            var updated = ContactRepository.Update(entity);

            return updated ? model : null;
        }

        public IEnumerable<SalesRep> GetSalesReps()
        {
            string sql = @"
select	id
		,first_name
		,last_name
		,email_address
from	contacts
where	isnull(salesrep, 'N') = 'Y'
		and delete_flag <> 'Y'
";

            try
            {
                using (var conn = DbConn.GetP21DbConnection())
                {
                    return conn.Query<SalesRep>(sql).ToList();

                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
