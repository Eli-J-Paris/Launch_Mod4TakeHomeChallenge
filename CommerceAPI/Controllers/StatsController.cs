using CommerceAPI.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly CommerceApiContext _context;

        public StatsController(CommerceApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult CountOfProducts()
        {
            var products = _context.Products;
            return new JsonResult(products);
        }

    }
}
