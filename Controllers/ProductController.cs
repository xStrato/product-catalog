using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels.ProductViewModels;

namespace ProductCatalog.Controllers
{    
    [Route("")]
    public class ProductController: Controller
    {
        private readonly ProductRepository _repository;
        public ProductController(ProductRepository repository) => _repository = repository; 

        [HttpGet]
        [Route("v1/products")]
        [ResponseCache(Duration=60, Location=ResponseCacheLocation.Any, VaryByHeader="")]
        public async Task<ActionResult<IEnumerable<ListProductViewModel>>> Get([FromServices]DataContext context) => await _repository.Get(context);

        [HttpGet]
        [Route("v1/products/{id:int}")]
        public async Task<ActionResult<Product>> Get(int id, [FromServices]DataContext context) => await _repository.Get(id, context);

        [HttpGet]
        [Route("v1/products/{id:int}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int id, [FromServices] DataContext context) => await context.Products.AsNoTracking().Where(x => x.Id.Equals(id)).ToListAsync();

        [HttpPost]
        [Route("v1/products")]
        public async Task<ActionResult<Product>> Post([FromBody] EditorProductViewModel model,[FromServices] DataContext context)
        {
            if(!ModelState.IsValid) return BadRequest(new { message = "model from body is invalid" });

            var product = new Product
            {
                Title = model.Title,
                CategoryId = model.CategoryId,
                CreateDate = DateTime.Now,
                Description = model.Description,
                Image = model.Image,
                LastUpdateDate = DateTime.Now,
                Price = model.Price,
                Quantity = model.Quantity
            };

            try
            {
                await _repository.Save(product, context);
            }
            catch(System.Exception error)
            {
                return BadRequest(new { message = error.Message });
            }
            return Ok(new { result = 200, message="Product successfully created", product=product });
        }

        [HttpPut]
        [Route("v1/products")]
        public async Task<ActionResult<Product>> Put([FromBody] EditorProductViewModel model,[FromServices] DataContext context)
        {
            if(!ModelState.IsValid) return BadRequest(new { message = "model from body is invalid" });

            Product product = null;

            try
            {
                product = context.Products.Find(model.Id); 
                product.Title = model.Title;
                product.CategoryId = model.CategoryId;
                // product.CreateDate = DateTime.Now;
                product.Description = model.Description;
                product.Image = model.Image;
                product.LastUpdateDate = DateTime.Now;
                product.Price = model.Price;
                product.Quantity = model.Quantity;

                await _repository.Update(product, context);
            }
            catch(System.Exception error)
            {
                return BadRequest(new { message = error.Message });
            }
            return Ok(new { result = 200, message="Product successfully created", product=product });
        }

        [HttpDelete]
        [Route("v1/products")]
        public async Task<ActionResult<Product>> Delete([FromBody] Product model,[FromServices] DataContext context)
        {
            if(!ModelState.IsValid) return BadRequest(new { message = "model from body is invalid" });

            try
            {
                context.Products.Remove(model);
                await context.SaveChangesAsync();
            }
            catch(System.Exception error)
            {
                return BadRequest(new { message = error.Message });
            }
            return Ok(new { message="Product removed successfully", product=model});
        }
    }
}