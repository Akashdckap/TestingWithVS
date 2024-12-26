using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P21_latest_template.Models;
using P21_latest_template.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")] //comment this to go into  debug mode thru swagger
    [Produces("application/json")]
    [Route("api/Payments")]
    public class PaymentsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IPaymentService _paymentService;

        public PaymentsController(
            ILogger<ProductsController> logger,
            IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        //TJSNOW
        [HttpGet("PaymentTypes")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PaymentType>))]
        public IActionResult PaymentTypes()
        {
            var data = _paymentService.GetPaymentTypes();
            return Ok(data);
        }
    }
}
