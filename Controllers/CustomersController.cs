using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using P21_latest_template.Models;
using P21_latest_template.Services;
using System.Collections.Generic;

namespace P21_latest_template.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")] //comment this to go into  debug mode thru swagger
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerService _customerService;
        private readonly IContactService _contactService;
        private readonly IAddressService _addressService;

        public CustomersController(

            ILoggerFactory loggerFactory,//This is a factory interface used to create instances of ILogger.
            ICustomerService customerService,
            IContactService contactService,
            IAddressService addressService)
        {
            _logger = loggerFactory.CreateLogger<CustomersController>();//this method creates logger speifically for the customercontroller class
            _customerService = customerService;
            _contactService = contactService;
            _addressService = addressService;
        }

        // MDS
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(CustomerCreate))]
        //public IActionResult Post([FromBody] CustomerCreate model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    if (model.customer_id != null)
        //    {
        //        var customer = _customerService.GetCustomerByp21ID((decimal)model.customer_id, model.customer_name, model.Contact);
        //        if (customer != null)
        //        {
        //            customer.company_id = "MMD";
        //            customer.customer_id = customer.customer_id;
        //            customer.customer_name = customer.customer_name;
        //            return Ok(customer);
        //        }
        //    }
        //    var data = _customerService.Create(model);
        //    if (data != null)
        //        return Ok(data);
        //    return BadRequest();
        //}
        public IActionResult Post([FromBody] CustomerCreate model)
        {
            _logger.LogDebug(JsonConvert.SerializeObject(model));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //var get_customer = _customerService.GetCustomerByEcommerceId(model.ecomerce_customer_id);
            //if (get_customer != null)
            //{
            //    model.Address.id = get_customer.customer_id; // customer id is same as address id
            //    model.customer_id = get_customer.customer_id;
            //    model.Contact.id = get_customer.id;
            //    return Put(model);
            //}

            _logger.LogDebug("checking");
            var customer = _customerService.GetCustomerByContactEmail(model.Contact.email_address, model.customer_name, model.Address.mail_state, model.Address.mail_postal_code, model.Address.phys_state, model.Address.phys_postal_code, model.Contact);
            _logger.LogDebug(Newtonsoft.Json.JsonConvert.SerializeObject(customer));
            if (customer != null)
            {
                customer.customer_id = customer.Address.id;
                customer.company_id = "MMD";
                customer.customer_name = customer.Address.name;
                //  customer.ecomerce_customer_id = model.ecomerce_customer_id;
                _logger.LogDebug(customer.salesrep_id);
                model.salesrep_id = customer.salesrep_id;
                _logger.LogDebug("Creating User Defined Values");
                //   _customerService.createUserDefinedValues(model.company_id, model.ecomerce_customer_id, Convert.ToDecimal(customer.customer_id));
                model.Address.id = customer.Address.id;
                model.customer_id = customer.customer_id;
                model.Contact.id = customer.Contact.id;
                return Ok(model);
            }
            var data = _customerService.Create(model);
            if (data != null)
                return Ok(data);

            return BadRequest();
        }

        // MDS
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(CustomerCreate))]
        public IActionResult Put([FromBody] CustomerCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contactUpdated = _contactService.Update(model.Contact);
            var addressUpdated = _addressService.Update(model.Address);

            if (contactUpdated == null)
                model.Contact = null;
            if (addressUpdated == null)
                model.Address = null;

            if (contactUpdated == null && addressUpdated == null)
                return BadRequest();
            else
                return Ok(model);
        }

        [HttpPost]
        [Route("Find")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.MDS.Customer>))]
        public IActionResult Find([FromBody] CustomerFilterParam model)
        {
            var results = new List<Models.MDS.Customer>();

            var data = _customerService.Find(model);

            if (data != null)
            {
                foreach (var d in data)
                {
                    var c = _customerService.GetCustomer(d.company_id, d.customer_id, d.Total);
                    if (c != null) results.Add(c);
                }
            }

            return Ok(results);
        }

        [HttpGet]
        [Route("CompanyId/{company_id}/CustomerId/{customer_id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Models.MDS.Customer>))]
        public IActionResult Get(string company_id, decimal customer_id)
        {
            var data = _customerService.GetCustomer(company_id, customer_id, 0);
            return Ok(data);
        }

        [HttpPost]
        [Route("CreateContact")]
        [ProducesResponseType(200, Type = typeof(CustomerContactCreate))]
        public IActionResult CreateContact([FromBody] CustomerContactCreate model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var contact = _customerService.CreateContact(model);
            if (contact != null && !string.IsNullOrEmpty(contact.id))
                return Ok(contact);
            return BadRequest();
        }

        [HttpGet]
        [Route("SalesReps")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SalesRep>))]
        public IActionResult SalesReps()
        {
            var data = _contactService.GetSalesReps();
            return Ok(data);
        }
    }
}
