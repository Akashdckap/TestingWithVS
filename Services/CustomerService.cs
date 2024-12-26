using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using P21_latest_template.Entities;
using P21_latest_template.Repositories;
using P21_latest_template.Helpers;
using P21_latest_template.Models;
using P21_latest_template.Models.MDS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P21_latest_template.Services
{
    public class CustomerService : ICustomerService
    {
        protected readonly IDbConnectionService DbConn;
        protected readonly ILogger<CustomerService> Logger;
        protected readonly ICustomerRepository CustomerRepository;
        protected readonly ICustomerSalesrepRepository CustomerSalesrepRepository;
        protected readonly IContactRepository ContactRepository;
        protected readonly IOeContactCustomerRepository OeContactCustomerRepository;
        protected readonly IAddressRepository AddressRepository;
        protected readonly IShipToRepository ShipToRepository;
        protected readonly IShipToSalesrepRepository ShipToSalesrepRepository;
        protected readonly ICustDefaultRepository CustDefaultRepository;
        protected readonly ICompanyRepository CompanyRepository;
        protected readonly ICounterService CounterService;
        protected readonly IPriceLibraryXCustXCmpyRepository PriceLibraryXCustXCmpyRepository;
        protected readonly IPriceLibraryRepository PriceLibraryRepository;
        protected readonly IDocumentLinkEntityReqRepository DocumentLinkEntityReqRepository;
        protected readonly ICorpIdRepository CorpIdRepository;

        public CustomerService(IDbConnectionService dbConn,
            ILoggerFactory loggerFactory,
            ICustomerRepository customerRepo,
            ICustomerSalesrepRepository customerSalesrepRepo,
            IContactRepository contactRepo,
            IOeContactCustomerRepository oeContactCustomerRepo,
            IAddressRepository addressRepo,
            IShipToRepository shipToRepo,
            IShipToSalesrepRepository shipToSalesrepRepo,
            ICustDefaultRepository custDefaultRepo,
            ICompanyRepository companyRepo,
            ICounterService counterService,
            IPriceLibraryXCustXCmpyRepository priceLibraryXCustXCmpyRepository,
            IPriceLibraryRepository priceLibraryRepo,
            IDocumentLinkEntityReqRepository docLinkEntityRepo,
            ICorpIdRepository corpIdRepo)
        {
            DbConn = dbConn;
            Logger = loggerFactory.CreateLogger<CustomerService>();
            CustomerRepository = customerRepo;
            CustomerSalesrepRepository = customerSalesrepRepo;
            ContactRepository = contactRepo;
            OeContactCustomerRepository = oeContactCustomerRepo;
            AddressRepository = addressRepo;
            ShipToRepository = shipToRepo;
            ShipToSalesrepRepository = shipToSalesrepRepo;
            CustDefaultRepository = custDefaultRepo;
            CompanyRepository = companyRepo;
            CounterService = counterService;
            PriceLibraryXCustXCmpyRepository = priceLibraryXCustXCmpyRepository;
            PriceLibraryRepository = priceLibraryRepo;
            DocumentLinkEntityReqRepository = docLinkEntityRepo;
            CorpIdRepository = corpIdRepo;
        }

        public CustomerCreate Create(CustomerCreate model)
        {
            //Logger.LogDebug("Create method");
            if (model == null)
            {
                Logger.LogDebug("CustomerCreate model is required");
                return null;
            }

            Logger.LogDebug($"getting cust default {model.company_id}");
            var custDefault = CustDefaultRepository.Get(model.company_id);
            if (custDefault == null)
            {
                Logger.LogError($"unable to retrieve cust default for company {model.company_id}");
                return null;
            }
            Logger.LogDebug($"getting company {model.company_id}");
            var company = CompanyRepository.Get(model.company_id);
            if (company == null)
            {
                Logger.LogError($"unable to retrieve company {model.company_id}");
                return null;
            }

            Logger.LogDebug("getting address id");
            var address_id = CounterService.GetNextCounter("ADDR");

            var address = new Address()
            {
                id = address_id,
                name = model.customer_name,
                corp_address_id = address_id,
                shipping_address = "Y"
            };

            Logger.LogDebug("set address default values");
            address.SetDefaultValues(model.Address, custDefault);
            var addressCreated = AddressRepository.Create(address);
            Logger.LogDebug($"create address {addressCreated}", address);
            if (!addressCreated)
            {
                Logger.LogError($"unable to create address", address);
                return null;
            }

            var customer = new Entities.Customer()
            {
                company_id = model.company_id,
                customer_id = address_id,
                customer_name = model.customer_name,
                customer_id_string = address_id.ToString("G29")
            };

            Logger.LogDebug("set customer default values");
            customer.SetDefaultValues(company, custDefault);
            var customerCreated = CustomerRepository.Create(customer);
            Logger.LogDebug($"create customer {customerCreated}", customer);
            if (!customerCreated)
            {
                Logger.LogError($"unable to create customer", customer);
                return null;
            }

            var docLinkEntityReq = new DocumentLinkEntityReq()
            {
                company_id = customer.company_id,
                document_link_entity_req_uid = CounterService.GetNextCounter("document_link_entity_req"),
                entity_id = (int)customer.customer_id,
            };

            var docLinkEntityReqCreated = DocumentLinkEntityReqRepository.Create(docLinkEntityReq);
            Logger.LogDebug($"create documentlinkentityreq {docLinkEntityReqCreated}", docLinkEntityReq);

            var corpId = new CorpId()
            {
                company_id = customer.company_id,
                address_id = customer.customer_id,
                address_name = customer.customer_name
            };

            var corpIdCreated = CorpIdRepository.Create(corpId);
            Logger.LogDebug($"create corpId {corpIdCreated}", corpId);

            Logger.LogDebug($"price_library_id '{custDefault.price_library_id}'");
            if (!string.IsNullOrEmpty(custDefault.price_library_id))
            {
                Logger.LogDebug($"get price lib {custDefault.price_library_id}");
                var price_lib = PriceLibraryRepository.GetByPriceLibraryId(custDefault.price_library_id);

                if (price_lib != null)
                {
                    Logger.LogDebug($"price library uid {price_lib.price_library_uid}");
                    var customerPriceLib = new PriceLibraryXCustXCmpy()
                    {
                        company_id = customer.company_id,
                        customer_id = customer.customer_id,
                        price_library_uid = price_lib.price_library_uid,
                        price_lib_x_cust_x_cmpy_uid = CounterService.GetNextCounter("price_library_x_cust_x_cmpy")
                    };
                    var customerPriceLibCreated = PriceLibraryXCustXCmpyRepository.Create(customerPriceLib);
                    Logger.LogDebug($"create pricelibaryxcustomerxcmpy {customerPriceLibCreated}", customerPriceLib);
                }
                else
                    Logger.LogDebug($"unable to retrieve price lib {custDefault.price_library_id}");
            }

            if (!string.IsNullOrEmpty(customer.salesrep_id))
            {
                var customerSalesrep = new CustomerSalesrep()
                {
                    company_id = customer.company_id,
                    customer_id = customer.customer_id,
                    salesrep_id = customer.salesrep_id,
                    customer_salesrep_uid = CounterService.GetNextCounter("customer_salesrep"),
                    primary_salesrep_flag = "Y"
                };
                var customerSalesrepCreated = CustomerSalesrepRepository.Create(customerSalesrep);
                Logger.LogDebug($"create customer salesrep {customerSalesrepCreated}", customerSalesrep);
            }

            var contact = new Contact()
            {
                id = CounterService.GetNextCounter("CONTC").ToString()
            };
            Logger.LogDebug("set contact default values");
            contact.SetDefaultValues(model.Contact, address.id);
            var contactCreated = ContactRepository.Create(contact);
            Logger.LogDebug($"create contact {contactCreated}", contact);

            var oeContactCustomer = new OeContactCustomer()
            {
                company_id = model.company_id,
                contact_id = contact.id,
                customer_id = customer.customer_id
            };

            var oeContactCustomerCreated = OeContactCustomerRepository.Create(oeContactCustomer);
            Logger.LogDebug($"create oecontactcustomer {oeContactCustomerCreated}", oeContactCustomer);

            var shipTo = new ShipTo()
            {
                company_id = customer.company_id,
                customer_id = customer.customer_id,
                ship_to_id = customer.customer_id
            };
            Logger.LogDebug("set shipto default values");
            shipTo.SetDefaultValues(customer, custDefault, company);
            var shipToCreated = ShipToRepository.Create(shipTo);
            Logger.LogDebug($"create shipto {shipToCreated}", shipTo);

            if (!string.IsNullOrEmpty(customer.salesrep_id))
            {
                var shiptoSalesrep = new ShipToSalesrep()
                {
                    company_id = shipTo.company_id,
                    ship_to_id = shipTo.ship_to_id,
                    salesrep_id = customer.salesrep_id,
                    primary_salesrep = "Y"
                };
                var shiptoSalesrepCreated = ShipToSalesrepRepository.Create(shiptoSalesrep);
                Logger.LogDebug($"create shiptosalesrep {shiptoSalesrepCreated}", shiptoSalesrep);
            }

            model.customer_id = customer.customer_id;
            model.Contact.id = contact.id;
            model.Address.id = shipTo.ship_to_id;
            model.salesrep_id = customer.salesrep_id;
            return model;
        }

        public IEnumerable<CustomerLite> Find(CustomerFilterParam param)
        {
            //with item as is CTE common table expression
            #region sql
            string sql = @"
;with items as
(
	select	c.company_id
			,c.customer_id
			,c.customer_name
            ,c.po_no_required
			,c.credit_limit
			,c.credit_limit_used
			,c.credit_limit_per_order
			,c.credit_status
			,c.delete_flag
            ,c.salesrep_id
			,row_num = row_number() over (order by c.customer_id)	
	from	customer c
	where	c.customer_type_cd = 1203
			and c.company_id = @company_id
			and 
			(
				c.date_last_modified >= @date_last_modified
				or c.customer_id in
				(
					select	s.customer_id
					from	ship_to s
							join [address] a on s.ship_to_id = a.id
					where	s.company_id = @company_id
							and
							(
								s.date_last_modified >= @date_last_modified
								or a.date_last_modified >= @date_last_modified
							)
					group by s.customer_id

					union

					select	ctc.customer_id
					from	oe_contacts_customer ctc
							join contacts ct on ctc.contact_id = ct.id
					where	ctc.company_id = @company_id
							and 
							(
								ctc.date_last_modified >= @date_last_modified
								or ct.date_last_modified >= @date_last_modified
							)
					group by ctc.customer_id
				)
			)
)
select		items.*
			,Total = cnt
from		items
			cross join (select cnt = count(1) from items) as cnt
--where		row_num between (@page_size * (@page_number - 1)) + 1 and (@page_number * @page_size)
order by	items.customer_id
offset @page_size * (@page_number - 1) rows
fetch next @page_size rows only;
";
            #endregion

            using (var conn = DbConn.GetP21DbConnection())
            {
                try
                {
                    conn.Open();
                    return conn.Query<CustomerLite>(sql, new
                    {
                        param.company_id,
                        param.page_number,
                        param.page_size,
                        param.date_last_modified
                    }).ToList();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return null;
        }
        public CustomerCreate GetCustomerByContactEmail(string email_address, string company_name, string mail_state, string mail_postal_code, string phys_state, string phys_postal_code, ContactCreate contact)
        {

            var customer = new CustomerCreate();
            #region sql
            string sql = @"select top 1
                                oe_contacts_customer.customer_id--, oe_contacts_customer.contact_id
                            from 
                                contacts 
                            inner join 
                                oe_contacts_customer 
                            on 
                                contacts.id = oe_contacts_customer.contact_id
                            inner join
                                address
                            on
                                address.id = oe_contacts_customer.customer_id
                                and address.delete_flag <> 'Y'
                               and address.name = @company_name
                                and ((mail_state = @mail_state and LEFT(mail_postal_code,5) = @mail_postal_code) or (phys_state = @phys_state and LEFT(phys_postal_code,5) = @phys_postal_code))
                            inner join
                                customer
                            on
                                customer.customer_id = oe_contacts_customer.customer_id
                                and customer.delete_flag <> 'Y'
                                and customer.customer_type_cd = 1203
                            where 
                                contacts.email_address = @email_address 
                                and oe_contacts_customer.delete_flag <> 'Y' 
                                and contacts.delete_flag <> 'Y'
                                    ";
            #endregion

            using (var conn = DbConn.GetP21DbConnection())
            {
                try
                {
                    conn.Open();
                    Logger.LogDebug("chckc 2");
                    var id = conn.Query<string>(sql, new { email_address, company_name, mail_postal_code, mail_state, phys_postal_code, phys_state }).FirstOrDefault();
                    if (id != null) Logger.LogDebug("id : " + id.ToString());

                    if (id == null)
                    {
                        Logger.LogDebug(company_name + mail_state + mail_postal_code + phys_state + phys_postal_code);
                        sql = @"select top 1 c.customer_id from address a inner join customer c on a.id = c.customer_id where name = @company_name and ((mail_state = @mail_state and mail_postal_code = @mail_postal_code) or (phys_state = @phys_state and phys_postal_code = @phys_postal_code)) and a.delete_flag<>'Y' and c.delete_flag<>'Y' order by id desc";
                        var address_id = conn.Query<decimal>(sql, new { company_name, mail_state, mail_postal_code, phys_state, phys_postal_code }).FirstOrDefault();
                        string sql1 = @"select top 1 c.salesrep_id from address a inner join customer c on a.id = c.customer_id where name = @company_name and ((mail_state = @mail_state and mail_postal_code = @mail_postal_code) or (phys_state = @phys_state and phys_postal_code = @phys_postal_code)) and a.delete_flag<>'Y' and c.delete_flag<>'Y' order by id desc";
                        var address_id1 = conn.Query<string>(sql1, new { company_name, mail_state, mail_postal_code, phys_state, phys_postal_code }).FirstOrDefault();
                        Logger.LogDebug("address_id: " + address_id.ToString());
                        if (address_id > Convert.ToDecimal(0))
                        {
                            if (contact != null)
                            {
                                var created_contact = new CustomerContactCreate
                                {
                                    first_name = contact.first_name,
                                    email_address = contact.email_address,
                                    direct_phone = contact.direct_phone,
                                    title = contact.title,
                                    last_name = contact.last_name,
                                    company_id = "MMD",
                                    customer_id = Convert.ToDecimal(address_id)
                                };
                                Logger.LogDebug(JsonConvert.SerializeObject(created_contact));
                                customer.Contact = CreateContact(created_contact);
                                customer.salesrep_id = address_id1;
                                Logger.LogDebug("eeee" + address_id1);

                            }
                            sql = @"select * from address where id = @address_id";
                            customer.Address = conn.Query<AddressCreate>(sql, new { address_id }).FirstOrDefault();
                            Logger.LogDebug("Customer with Address:" + JsonConvert.SerializeObject(customer));
                            return customer;
                        }
                        Logger.LogDebug("returning null");
                        return null;
                    }
                    else
                    {
                        string retrive_customer = @"
                                                select * from customer where customer_id = @customer_id;
                                                select top 1  * from contacts where email_address = @email_address 
                                                select * from address where id = @customer_id";
                        var multi = conn.QueryMultiple(sql: retrive_customer, new { customer_id = id, email_address });
                        customer = multi.Read<CustomerCreate>().FirstOrDefault();
                        customer.Contact = multi.Read<ContactCreate>().FirstOrDefault();
                        Logger.LogDebug("Contact" + JsonConvert.SerializeObject(customer.Contact));
                        customer.Address = multi.Read<AddressCreate>().FirstOrDefault();
                        Logger.LogDebug("Address" + JsonConvert.SerializeObject(customer.Address));
                        Logger.LogDebug("Customer" + JsonConvert.SerializeObject(customer));
                        return customer;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            //var customer = new CustomerCreate();
            //#region sql
            //    string sql = @"select 
            //                        oe_contacts_customer.customer_id 
            //                   from 
            //                        contacts 
            //                   inner join 
            //                        oe_contacts_customer on contacts.id = oe_contacts_customer.contact_id 
            //                   inner join 
            //                        customer on oe_contacts_customer.customer_id = customer.customer_id and customer.customer_type_cd = 1203
            //                   where 
            //                        contacts.email_address = @email_address 
            //                        and oe_contacts_customer.delete_flag <> 'Y' 
            //                        and contacts.delete_flag <> 'Y' 
            //                        and customer.delete_flag <> 'Y'";
            //#endregion

            //using (var conn = DbConn.GetP21DbConnection())
            //{
            //    try
            //    {
            //        conn.Open();
            //        var id = conn.Query<string>(sql,new { email_address}).FirstOrDefault();
            //        if(id!=null)Logger.LogDebug("id : " + id.ToString());

            //        if (id == null)
            //        {
            //            Logger.LogDebug(company_name + mail_state + mail_postal_code + phys_state + phys_postal_code);
            //            sql = @"select top 1 id from address a inner join customer c on a.id = c.customer_id and c.customer_type_cd = 1203 where name = @company_name and ((mail_state = @mail_state and mail_postal_code = @mail_postal_code) or (phys_state = @phys_state and phys_postal_code = @phys_postal_code)) and a.delete_flag<>'Y' and c.delete_flag<>'Y' order by id desc";
            //            var address_id = conn.Query<decimal>(sql, new { company_name, mail_state, mail_postal_code, phys_state, phys_postal_code }).FirstOrDefault();
            //            Logger.LogDebug("address_id: " + address_id.ToString());
            //            if (address_id > Convert.ToDecimal(0))
            //            {
            //                if (contact != null)
            //                {
            //                    var created_contact = new CustomerContactCreate
            //                    {
            //                        first_name = contact.first_name,
            //                        email_address = contact.email_address,
            //                        direct_phone = contact.direct_phone,
            //                        title = contact.title,
            //                        last_name = contact.last_name,
            //                        company_id = "ASG",
            //                        customer_id = Convert.ToDecimal(address_id)
            //                    };
            //                    Logger.LogDebug(JsonConvert.SerializeObject(created_contact));
            //                    customer.Contact = CreateContact(created_contact);

            //                }
            //                sql = @"select * from address where id = @address_id";
            //                customer.Address = conn.Query<AddressCreate>(sql, new { address_id }).FirstOrDefault();
            //                Logger.LogDebug("Customer with Address:" + JsonConvert.SerializeObject(customer));
            //                return customer;
            //            }
            //            Logger.LogDebug("returning null");
            //            return null;
            //        }
            //        else
            //        {
            //            string retrive_customer = @"
            //                                    select * from contacts where email_address = @email_address and address_id = @id
            //                                    select * from address where id = @id";
            //            var multi = conn.QueryMultiple(sql: retrive_customer, new { id, email_address });
            //            customer.Contact = multi.Read<ContactCreate>().FirstOrDefault();
            //            Logger.LogDebug("Contact" + JsonConvert.SerializeObject(customer.Contact));
            //            customer.Address = multi.Read<AddressCreate>().FirstOrDefault();
            //            Logger.LogDebug("Address" + JsonConvert.SerializeObject(customer.Address));
            //            Logger.LogDebug("Customer" + JsonConvert.SerializeObject(customer));
            //            return customer;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Logger.LogError(ex.Message);
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }
            //}

            return null;
        }
        public CustomerCreate GetCustomerByp21ID(decimal customer_id, string company_name, ContactCreate contact)
        {
            var customer = new CustomerCreate();
            #region sql
            string sql = @"select customer_id from customer where customer_id=@customer_id";
            #endregion

            using (var conn = DbConn.GetP21DbConnection())
            {
                try
                {
                    conn.Open();
                    var id = conn.Query<string>(sql, new { customer_id }).FirstOrDefault();
                    if (id != null) Logger.LogDebug("id : " + id.ToString());

                    if (id != null)
                    {
                        Logger.LogDebug("new contact creation");
                        //Logger.LogDebug(company_name + mail_state + mail_postal_code + phys_state + phys_postal_code);
                        //sql = @"select top 1 c.customer_id from address a inner join ship_to s on a.id = s.ship_to_id inner join customer c on (a.id = c.customer_id or s.customer_id = c.customer_id) where name = @company_name and ((mail_state = @mail_state and mail_postal_code = @mail_postal_code) or (phys_state = @phys_state and phys_postal_code = @phys_postal_code)) and a.delete_flag<>'Y' and c.delete_flag<>'Y' order by id desc";
                        //var address_id = conn.Query<decimal>(sql, new { company_name, mail_state, mail_postal_code, phys_state, phys_postal_code }).FirstOrDefault();
                        //Logger.LogDebug("address_id: " + address_id.ToString());
                        //if (address_id > Convert.ToDecimal(0))
                        //{
                        if (contact != null)
                        {
                            var created_contact = new CustomerContactCreate
                            {
                                first_name = contact.first_name,
                                email_address = contact.email_address,
                                direct_phone = contact.direct_phone,
                                title = contact.title,
                                last_name = contact.last_name,
                                company_id = "MMD",
                                customer_id = customer_id
                            };
                            Logger.LogDebug(JsonConvert.SerializeObject(created_contact));
                            customer.Contact = CreateContact(created_contact);

                            // }
                            sql = @"select * from address where id = @customer_id";
                            customer.Address = conn.Query<AddressCreate>(sql, new { customer_id }).FirstOrDefault();
                            Logger.LogDebug("Customer with Address:" + JsonConvert.SerializeObject(customer));
                            return customer;
                        }
                        Logger.LogDebug("returning null");
                        return null;
                    }
                    //else
                    //{
                    //    string retrive_customer = @"
                    //                            select * from contacts where email_address = @email_address
                    //                            select * from address where id = @id";
                    //    var multi = conn.QueryMultiple(retrive_customer, new { id, email_address });
                    //    customer.Contact = multi.Read<ContactCreate>().FirstOrDefault();
                    //    Logger.LogDebug("Contact" + JsonConvert.SerializeObject(customer.Contact));
                    //    customer.Address = multi.Read<AddressCreate>().FirstOrDefault();
                    //    Logger.LogDebug("Address" + JsonConvert.SerializeObject(customer.Address));
                    //    Logger.LogDebug("Customer" + JsonConvert.SerializeObject(customer));
                    //    return customer;
                    //}
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return null;
        }
        public Models.MDS.Customer GetCustomer(string company_id, decimal customer_id, int total)
        {
            #region sql
            string sql = @"
select	c.company_id
		,c.customer_id
		,c.customer_name
		,c.credit_limit
		,c.credit_limit_used
		,c.credit_limit_per_order
		,c.credit_status
		,c.delete_flag
        ,Total=@total
from	customer c
where	c.customer_type_cd = 1203
		and c.company_id = @company_id
		and c.customer_id = @customer_id

select	a.id
		,a.name
		,a.mail_address1
		,a.mail_address2
		,a.mail_city
		,a.mail_state
		,a.mail_postal_code
		,a.mail_country
		,a.phys_address1
		,a.phys_address2
		,a.phys_city
		,a.phys_state
		,a.phys_postal_code
		,a.phys_country
		,a.central_phone_number
		,delete_flag = case when a.delete_flag = 'Y' or st.delete_flag = 'Y' then 'Y' else 'N' end
from	ship_to st
		join address a on st.ship_to_id = a.id
where	st.company_id = @company_id
		and st.customer_id = @customer_id

select	ct.id
		,ct.first_name
		,ct.last_name
		,ct.title
		,ct.direct_phone
		,ct.email_address
		,delete_flag = case when cc.delete_flag = 'Y' or ct.delete_flag = 'Y' then 'Y' else 'N' end
from	oe_contacts_customer cc
		join contacts ct on cc.contact_id = ct.id
where	cc.company_id = @company_id
		and cc.customer_id = @customer_id
";
            #endregion

            using (var conn = DbConn.GetP21DbConnection())
            {
                try
                {
                    conn.Open();
                    using (var multi = conn.QueryMultiple(sql, new { company_id, customer_id, total }))
                    {
                        var data = multi.Read<Models.MDS.Customer>().FirstOrDefault();
                        if (data == null) return null;
                        data.Addresses = multi.Read<CustomerAddress>().ToList();
                        data.Contacts = multi.Read<CustomerContact>().ToList();
                        return data;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            return null;
        }

        public CustomerContactCreate CreateContact(CustomerContactCreate model)
        {
            var contact = new Contact()
            {
                id = CounterService.GetNextCounter("CONTC").ToString()
            };
            Logger.LogDebug("set contact default values");
            contact.SetDefaultValues(model, model.customer_id);
            var contactCreated = ContactRepository.Create(contact);
            Logger.LogDebug($"create contact {contactCreated}", contact);

            var oeContactCustomer = new OeContactCustomer()
            {
                company_id = model.company_id,
                contact_id = contact.id,
                customer_id = model.customer_id
            };
            var oeContactCustomerCreated = OeContactCustomerRepository.Create(oeContactCustomer);
            Logger.LogDebug($"create oecontactcustomer {oeContactCustomerCreated}", oeContactCustomer);
            model.id = contact.id;
            return model;
        }
    }
}
