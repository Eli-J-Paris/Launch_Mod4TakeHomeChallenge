using CommerceAPI.DataAccess;
using CommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CommerceAPI.Controllers
{
    [Route("api/merchants/{merchantId}/[Controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CommerceApiContext _context;

        public ProductsController(CommerceApiContext context)
        {
            _context = context;
        }
        //Create a new product associated with a specific merchant
        //Retrieve a specific product by its primary key
        //Update an existing product
        //Delete a product by its primary key

        [HttpGet("{id}")]
        public ActionResult GetProduct(int id)
        {
            var product = _context.Products.Find(id);
            return new JsonResult(product);
        }
    }
}
