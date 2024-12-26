using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace P21_latest_template
{
    public class ApiExplorerVisibilityEnabledConvention : IApplicationModelConvention
    {
        private readonly IConfiguration _configuration;
        private readonly string _licensedClient;
        private readonly IEnumerable<string> _services = new List<string>();

        private Dictionary<string, IEnumerable<string>> _dictionary = new Dictionary<string, IEnumerable<string>>()
        {
            { "TJSNOW", new string[]
                {
                    "Orders.PickTickets",
                    "Orders.Invoices",
                    "Payments.PaymentTypes",
                    "Products.FindByDateLastModified",
                    "Customers.SalesReps"
                }
            },
            { "MDS", new string[]
                {
                    "Orders.Import",
                    "Orders.Invoices",
                    "Products.FindByDateLastModified",
                    "Products.InventoryByDateLastModified",
                    "Products.Price",
                    "Customers.Post",
                    "Customers.Put",
                    "Customers.Find",
                    "Customers.Get",
                    "Customers.CreateContact",
                    "Customers.SalesReps"
                }
            },
            { "BTX", new string[]
                {
                    "Orders.Import",
                    "Orders.Invoices",
                    "Products.FindByDateLastModified",
                    "Products.InventoryByDateLastModified",
                    "Products.Price",
                    "Customers.Post",
                    "Customers.Put",
                    "Customers.Find",
                    "Customers.Get",
                    "Customers.CreateContact",
                    "Customers.SalesReps"
                }
            }
        };

        public ApiExplorerVisibilityEnabledConvention(IConfiguration configuration)
        {
            _configuration = configuration;
            _licensedClient = configuration["LicensedClient"];
            configuration.GetSection("Services").Bind(_services);
        }

        public void Apply(ApplicationModel application)
        {
            //if (!_dictionary.ContainsKey(_licensedClient))
            //    return;

            //var client = _dictionary[_licensedClient];

            foreach (var controller in application.Controllers)
            {
                if (controller.ApiExplorer.IsVisible != null) continue;

                foreach (ActionModel action in controller.Actions)
                {
                    if (action.ApiExplorer.IsVisible != null) continue;
                    //var found = client.Any(_ => _ == $"{controller.ControllerName}.{action.ActionName}");
                    var found = _services.Any(_ => _ == $"{controller.ControllerName}.{action.ActionName}");
                    action.ApiExplorer.IsVisible = found;
                }
            }
        }
    }
}
