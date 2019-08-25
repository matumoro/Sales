﻿namespace Sales.API.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Common.Models;
    using Domain.Models;
    using Sales.API.Helpers;

    public class ProductsController : ApiController
    {
        private readonly DataContext db = new DataContext();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return this.db.Products.OrderBy(p=>p.Description);
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            if (product.ImageArray != null && product.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(product.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                //var folder = "wwwroot\\images\\Products";
                var folder = "~/Content/Products";
                //var fullPath = $"~/images/Products/{file}";
                var fullPath = $"{folder}/{file}";

                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    //imageUrl = fullPath;
                    product.ImagePath = fullPath;
                }
            }

            this.db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product);
        }

        // POST: api/Products
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            product.IsAvailable = true;
            product.PublishOn = DateTime.Now.ToUniversalTime();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (product.ImageArray != null && product.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(product.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                //var folder = "wwwroot\\images\\Products";
                var folder = "~/Content/Products";
                //var fullPath = $"~/images/Products/{file}";
                var fullPath = $"{folder}/{file}";

                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    //imageUrl = fullPath;
                    product.ImagePath = fullPath;
                }
            }
                

            this.db.Products.Add(product);
            await this.db.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            this.db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}