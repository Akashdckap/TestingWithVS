using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using P21_latest_template.Models;
using P21_latest_template.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace P21_latest_template.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")] //comment this to go into  debug mode thru swagger
    [Produces("application/json")]
    [Route("api/Orders")]
    public class OrdersController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly IOrderImportService _orderImportService;
        private readonly IConfiguration _configuration;

        public OrdersController(
            ILogger<OrdersController> logger,
            IOrderService orderService,
            IOrderImportService orderImportService,
            IConfiguration configuration)
        {
            _logger = logger;
            _orderService = orderService;
            _orderImportService = orderImportService;
            _configuration = configuration;
        }

        //TJSNOW
        [HttpGet("{id}/PickTickets")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderPickTicket>))]
        public IActionResult GetPickTickets(string id)
        {
            var data = _orderService.GetPickTicketByOrderNo(id);
            return Ok(data);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpGet("AgingReport/{customer_id}")]
        [ProducesResponseType(200, Type = typeof(GetAgingReport))]
        public IActionResult GetAgingReport(decimal customer_id)
        {
            string year = DateTime.UtcNow.Year.ToString();
            string period = DateTime.UtcNow.Month.ToString();
            var tt = year + period.PadLeft(3, '0');
            return Ok(_orderService.GetAgingReports(customer_id, (year + period.PadLeft(3, '0'))));
        }

        //TJSNOW
        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpPost("Invoices")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<InvoiceShipment>))]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetInvoices([FromBody] InvoiceFilterParam param)
        {
            if (!param.from_date.HasValue && (param.order_ids == null || !param.order_ids.Any()))
            {
                return BadRequest("please provide from_date or order_ids");
            }

            //IEnumerable<InvoiceShipment> data = new List<InvoiceShipment>();

            //if (param.order_ids != null && param.order_ids.Any())
            //{
            //    data = _orderService.GetInvoiceByDateLastModified(param.company_id,
            //       null, null, param.order_ids);
            //}
            //else if (param.from_date.HasValue)
            //{
            //    data = _orderService.GetInvoiceByDateLastModified(param.company_id,
            //       param.from_date, param.to_date, new List<string>());
            //}

            IEnumerable<Order> data = new List<Order>();

            if (param.order_ids != null && param.order_ids.Any())
            {
                data = _orderService.Find(param.company_id,
                   null, null, param.order_ids);
            }
            else if (param.from_date.HasValue)
            {
                data = _orderService.Find(param.company_id,
                   param.from_date, param.to_date, new List<string>());
            }

            return Ok(data);
        }

        //[ApiExplorerSettings(IgnoreApi = false)]
        //[HttpPost("SalesForceInvoices")]
        ////[ProducesResponseType(200, Type = typeof(IEnumerable<InvoiceShipment>))]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<SalesForceOrder>))]
        //public IActionResult GetSalesForceInvoices([FromBody] InvoiceFilterParam param)
        //{
        //    if (!param.from_date.HasValue && (param.order_ids == null || !param.order_ids.Any()))
        //    {
        //        return BadRequest("please provide from_date or order_ids");
        //    }

        //    //IEnumerable<InvoiceShipment> data = new List<InvoiceShipment>();

        //    //if (param.order_ids != null && param.order_ids.Any())
        //    //{
        //    //    data = _orderService.GetInvoiceByDateLastModified(param.company_id,
        //    //       null, null, param.order_ids);
        //    //}
        //    //else if (param.from_date.HasValue)
        //    //{
        //    //    data = _orderService.GetInvoiceByDateLastModified(param.company_id,
        //    //       param.from_date, param.to_date, new List<string>());
        //    //}

        //    IEnumerable<SalesForceOrder> data = new List<SalesForceOrder>();

        //    if (param.order_ids != null && param.order_ids.Any())
        //    {
        //        data = _orderService.SalesForceFind(param.company_id,
        //           null, null, param.order_ids, param.page_number, param.page_size, param.line_count);
        //    }
        //    else if (param.from_date.HasValue)
        //    {
        //        data = _orderService.SalesForceFind(param.company_id,
        //           param.from_date, param.to_date, new List<string>(), param.page_number, param.page_size, param.line_count);
        //    }

        //    return Ok(data);
        //}


        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpGet("invoice_details/{invoice_no}")]
        [ProducesResponseType(200, Type = typeof(InvoiceDetails))]
        public IActionResult GetInvoiceDetails(string invoice_no)
        {
            return Ok(_orderService.GetInvoiceInfo(invoice_no));
        }

        //[ApiExplorerSettings(IgnoreApi = false)]
        //[HttpPost("Find")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        //public IActionResult Find([FromBody] OrderFilterParam param)
        //{
        //    if (!param.from_date.HasValue && (param.order_ids == null || !param.order_ids.Any()))
        //    {
        //        return BadRequest("please provide from_date or order_ids");
        //    }

        //    IEnumerable<Order> data = new List<Order>();

        //    if (param.order_ids != null && param.order_ids.Any())
        //    {
        //        data = _orderService.Find(param.company_id,
        //           null, null, param.order_ids);
        //    }
        //    else if (param.from_date.HasValue)
        //    {
        //        data = _orderService.Find(param.company_id,
        //           param.from_date, param.to_date, new List<string>());
        //    }
        //    return Ok(data);
        //}

        // MDS
        [HttpPost("Import")]
        [ProducesResponseType(200)]
        public IActionResult Import([FromBody] OrderImport param)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ImportFiles");

            var files = _orderImportService.ImportOrder(path, param);

            if (files == null || !files.Any())
            {
                return BadRequest();
            }

            foreach (var file in files)
            {
                //Getting the access to access the file
                FileInfo fileInfo = new FileInfo(file);
                fileInfo.MoveTo(file.Replace(".tmp", ".txt"));
            }

            return Ok(true);
        }

        [ApiExplorerSettings(IgnoreApi = false)]
        [HttpGet("{order_no}")]
        [ProducesResponseType(200, Type = typeof(OrderDetails))]
        public IActionResult GetOrderDetail(string order_no)
        {
            var data = _orderService.GetOrderInfo(order_no);
            return Ok(data);
        }



        //[ApiExplorerSettings(IgnoreApi = false)]
        //[HttpPost("GetCustomerOrderQuoteInfo")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Order_Header>))]
        //public IActionResult GetCustomerOrderQuoteInfo([FromBody] OfflineQuoteFilterParam model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    //IEnumerable<QuoteHeader> data = new List<QuoteHeader>();

        //    var data = _orderService.GetCustomerOrderDetails(model);

        //    return Ok(data);
        //}
    }
}