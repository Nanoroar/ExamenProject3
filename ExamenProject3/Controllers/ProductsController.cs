using ExamenProject3.Data;
using ExamenProject3.Filters;
using ExamenProject3.Models.Product;
using ExamenProject3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamenProject3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

       

        public ProductsController(IProductService productService)
        {
            _productService = productService;
           
        }

        [HttpGet, AllowAnonymous]

        public async Task<IActionResult> GetAll()
        {
            return new OkObjectResult(await _productService.GetAllAsync());
        }


        [HttpGet("{artnr}")]
        public async Task<IActionResult> Get(string artnr)
        {
            if (await _productService.GetProductAsync(artnr) != null)
                return new OkObjectResult(await _productService.GetProductAsync(artnr));
            else
                return new NotFoundResult();
        }

        
        [UseAdminApiKey]
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var result = await _productService.CreateAsync(product);
            if (result != null)
                return new OkObjectResult(result);

            return new BadRequestResult();
        }

        [UseAdminApiKey]
        [HttpPut("artnr")]

        public async Task<IActionResult> Update(string artnr, ProductUpdate product)
        {
            if (await _productService.UpdateProductAsync(artnr, product) != null)
            {
                return new OkObjectResult(product);
            }
            else
                return new NotFoundResult();
        }

        [UseAdminApiKey]
        [HttpDelete("{artnr}")]
        public async Task<IActionResult> Delete(string artnr)
        {
           
            if(await _productService.DeleteAsync(artnr))
                return new OkResult();
            else
                return new NotFoundResult();
        }


        
    }
}
