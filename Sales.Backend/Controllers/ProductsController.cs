﻿namespace Sales.Backend.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Backend.Models;
    using Common.Models;
    using System.Linq;
    using Sales.Backend.Helpers;
    using System.Web.UI.WebControls;
    using System;

    public class ProductsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await this.db.Products.OrderBy(p=>p.Description).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product ProductView = await this.db.Products.FindAsync(id);
            if (ProductView == null)
            {
                return HttpNotFound();
            }
            return View(ProductView);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView view )
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Products";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var product = this.ToProducts(view, pic);

                this.db.Products.Add(product);
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return base.View(view);
        }

        private Product ToProducts(ProductView view, string pic)
        {
            return new Product
            {
                Description = view.Description,
                ImagePath = pic,
                IsAvailable = view.IsAvailable,
                Price = view.Price,
                ProductId=view.ProductId,
                PublishOn = view.PublishOn,
                Remarks=view.Remarks,

            };
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = await db.Products.FindAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            var view = this.ToView(product);

            return View(view);
        }

        private ProductView ToView(Product product)
        {
            return new ProductView
            {
                Description = product.Description,
                ImagePath = product.ImagePath,
                IsAvailable = product.IsAvailable,
                Price = product.Price,
                ProductId = product.ProductId,
                PublishOn = product.PublishOn,
                Remarks = product.Remarks,

            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.ImagePath;
                var folder = "~/Content/Products";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var product = this.ToProducts(view, pic);

                this.db.Entry(product).State=EntityState.Modified;
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            
             return base.View(view);
            
        }
       

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await this.db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await this.db.Products.FindAsync(id);
            this.db.Products.Remove(product);
            await this.db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
