using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers
{
    public class CategoryController: Controller
    {
        [HttpGet]
        [Route("v1/categories")]
        public async Task<ActionResult<IEnumerable<Category>>> Get([FromServices] DataContext context) => await context.Categories.AsNoTracking().ToListAsync(); 
    }
}