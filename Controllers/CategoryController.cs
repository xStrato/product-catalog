using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers
{
    [Route("")]
    public class CategoryController: Controller
    {
        [HttpGet]
        [Route("v1/categories")]
        public async Task<ActionResult<IEnumerable<Category>>> Get([FromServices] DataContext context) => await context.Categories.AsNoTracking().ToListAsync(); 

        [HttpGet]
        [Route("v1/categories/{id:int}")]
        public async Task<ActionResult<Category>> Get(int id, [FromServices] DataContext context) => await context.Categories.AsNoTracking().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        [HttpGet]
        [Route("v1/categories/{id:int}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int id, [FromServices] DataContext context) => await context.Products.AsNoTracking().Where(x => x.Category.Id.Equals(id)).ToListAsync();

        [HttpPost]
        [Route("v1/categories")]
        public async Task<ActionResult<Category>> Post([FromBody] Category model,[FromServices] DataContext context)
        {
            if(!ModelState.IsValid) return BadRequest(new { message = "model from body is invalid" });

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
            }
            catch(System.Exception error)
            {
                return BadRequest(new { message = error.Message });
            }
            return Ok(model);
        }

        [HttpPut]
        [Route("v1/categories")]
        public async Task<ActionResult<Category>> Put([FromBody] Category model,[FromServices] DataContext context)
        {
            if(!ModelState.IsValid) return BadRequest(new { message = "model from body is invalid" });

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch(System.Exception error)
            {
                return BadRequest(new { message = error.Message });
            }
            return Ok(model);
        }

        [HttpDelete]
        [Route("v1/categories")]
        public async Task<ActionResult<Category>> Delete([FromBody] Category model,[FromServices] DataContext context)
        {
            if(!ModelState.IsValid) return BadRequest(new { message = "model from body is invalid" });

            try
            {
                context.Categories.Remove(model);
                await context.SaveChangesAsync();
            }
            catch(System.Exception error)
            {
                return BadRequest(new { message = error.Message });
            }
            return Ok(new { message="Category removed successfully", category=model});
        }
        
        [HttpDelete]
        [Route("v1/categories/{id:int}")]
        public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context)
        {
            Category category = null;
            try
            {
                category = await context.Categories.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if(category.Equals(null)) return NotFound(new { message="Category was not found" });

                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
            catch(System.Exception error)
            {
                return BadRequest(new { message = error.Message });
            }
            return Ok(new { message="Category removed successfully", category=category });
        }
    }
}