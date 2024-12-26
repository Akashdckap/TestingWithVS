using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P21_latest_template.Models;
using P21_latest_template.Services;
using System.Collections.Generic;

namespace P21_latest_template.Controllers
{
    // [Authorize(AuthenticationSchemes = "Bearer")] //comment this to go into  debug mode thru swagger
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly ILogger _logger;
        //The controller uses dependency injection to interact with a service (IProductService) that handles the logic for retrieving product data.
        private readonly IProductService _productService;

        public ProductsController(
            ILogger<ProductsController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        // TJSNOW
        [HttpPost("FindByDateLastModified")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductLite>))]
        public IActionResult FindByDateLastModified([FromBody] ProductFilterParam model)
        {
            var data = _productService.FindByDateLastModified(model);

            if (data != null && model.customer_id.HasValue && model.location_id.HasValue)
            {
                foreach (var product in data)
                {
                    var found = _productService.GetProductFound(product.inv_mast_uid,
                        "", product.default_selling_unit, model.location_id.Value);

                    if (product == null)
                    {
                        _logger.LogDebug($"skip due to {product.item_id} / {product.inv_mast_uid} not found");
                        continue;
                    }

                    var price = _productService.GetProductPrice(model.company_id,
                        model.customer_id.Value, found, 1);
                    if (price != null)
                        product.unit_price = price.unit_price;
                }
            }

            return Ok(data);
        }

        // MDS
        [HttpPost("InventoryByDateLastModified")]
        [ProducesResponseType(200, Type = typeof(ProductAvailability))]
        public IActionResult InventoryByDateLastModified([FromBody] ProductInventoryParam model)
        {
            var data = _productService.GetInventories(model.location_ids, model.date_last_modified, model.product_type);
            return Ok(new ProductAvailability() { ItemAvailability = data });
        }

        [HttpPost("Price")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductPrice>))]
        //Working on the pricing API second Time Testing with branch
        //Working on the Stash
        //THird attempt for testing branch
        // CHecking
        public IActionResult Price([FromBody] CustomerPriceParam model)
        {
            var results = new List<ProductPrice>();

            foreach (var product in model.Products)
            {
                var found = _productService.GetProductFound(product.inv_mast_uid ?? 0,
                    product.item_id ?? "", product.uom, model.location_id);

                if (product == null)
                {
                    _logger.LogDebug($"skip due to {product.item_id} / {product.inv_mast_uid} not found");
                    continue;
                }

                var price = _productService.GetProductPrice(model.company_id,
                    model.customer_id, found, product.qty);
                if (price != null)
                    results.Add(price);
            }

            return Ok(results);
        }
    }
}