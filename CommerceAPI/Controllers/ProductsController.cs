using CommerceAPI.DataAccess;
using CommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        
        //Update an existing product
        //Delete a product by its primary key


        [HttpGet]
        public ActionResult GetAllProducts()
        {
            var product = _context.Products;
            return new JsonResult(product);
        }


        [HttpGet("{id}")]
        public ActionResult GetSingleProduct(int id)
        {
            var product = _context.Products.Find(id);
            return new JsonResult(product);
        }

        [HttpPost]
        public ActionResult CreateProduct(int merchantId, Product product)
        {
            var merchant = _context.Merchants.Where(m => m.Id == merchantId).Include(p => p.Products).First();
            merchant.Products.Add(product);
            _context.SaveChanges();
            return new JsonResult(product);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, Product product)
        {
            product.Id = id;
            _context.Products.Update(product);
            _context.SaveChanges();
            return new JsonResult(product);
        }
    }
}
