using P21_latest_template.Models;
using System.Collections.Generic;

namespace P21_latest_template.Services
{
    public interface IContactService
    {
        ContactCreate Update(ContactCreate model);
        IEnumerable<SalesRep> GetSalesReps();
    }
}