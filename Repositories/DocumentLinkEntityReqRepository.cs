using Microsoft.Extensions.Logging;
using P21_latest_template.Repositories;
using P21_latest_template.Services;
using P21_latest_template.Entities;

namespace P21_latest_template.Repositories
{
    public interface IDocumentLinkEntityReqRepository : IRepository<DocumentLinkEntityReq, int>
    {

    }

    public class DocumentLinkEntityReqRepository : GenericRepository<DocumentLinkEntityReq>, IDocumentLinkEntityReqRepository
    {
        public DocumentLinkEntityReqRepository(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory)
            : base(dbConn, loggerFactory)
        {
        }
    }
}
