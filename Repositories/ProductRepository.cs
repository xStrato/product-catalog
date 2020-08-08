using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Repositories
{
    public class ProductRepository: Controller
    {
        public async Task<ActionResult<IEnumerable<ListProductViewModel>>> Get([FromServices]DataContext context) => await context.Products.Include(x=>x.Category).Select(x => new ListProductViewModel {
            Id = x.Id,
            Title = x.Title,
            Price = x.Price,
            Category = x.Category.Title,
            CategoryId = x.CategoryId
        }).AsNoTracking().ToListAsync();

        public async Task<ActionResult<Product>> Get(int id, [FromServices] DataContext context) => await context.Products.AsNoTracking().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        public async Task<ActionResult<Product>>  Save(Product product, [FromServices]DataContext context)
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task<ActionResult<Product>> Update(Product product, [FromServices]DataContext context)
        {
            context.Entry<Product>(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return product;
        }
    }
}