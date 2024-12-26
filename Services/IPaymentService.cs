using System.Collections.Generic;
using P21_latest_template.Models;

namespace P21_latest_template.Services
{
    public interface IPaymentService
    {
        IEnumerable<PaymentType> GetPaymentTypes();
    }
}