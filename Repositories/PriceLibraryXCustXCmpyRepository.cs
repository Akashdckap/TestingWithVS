using Microsoft.Extensions.Logging;
using P21_latest_template.Repositories;
using P21_latest_template.Services;
using P21_latest_template.Entities;
using System;

namespace P21_latest_template.Repositories
{
    public interface IPriceLibraryXCustXCmpyRepository : IRepository<PriceLibraryXCustXCmpy, int>
    {

    }

    public class PriceLibraryXCustXCmpyRepository : GenericRepository<PriceLibraryXCustXCmpy>, IPriceLibraryXCustXCmpyRepository
    {
        public PriceLibraryXCustXCmpyRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }

        public override bool Create(PriceLibraryXCustXCmpy entity)
        {
            entity.date_created = DateTime.Now;
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Create(entity);
        }

        public override bool Update(PriceLibraryXCustXCmpy entity)
        {
            entity.date_last_modified = DateTime.Now;

            if (string.IsNullOrEmpty(entity.last_maintained_by))
                entity.last_maintained_by = "admin";

            return base.Update(entity);
        }
    }
}
