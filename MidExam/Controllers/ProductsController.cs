using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidExam.Data;
using MidExam.Models;
using MidExam.ViewModels;

namespace MidExam.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;
        private IWebHostEnvironment w;

        public ProductsController(ApplicationDbContext _db, IWebHostEnvironment _w)
        {
            db = _db;
            w = _w;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<ProductVM> pVMs = db.Products.Select(p => new ProductVM(p.Id, p.Name, p.ImageUrl));
            return View(await pVMs.ToListAsync());
        }

        [HttpPost]
        public async Task<string> GetImageUrl(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                string FN = image.FileName;
                string FP = w.WebRootPath + $@"\images\{FN}";
                //long size = image.Length;

                using (FileStream fs = System.IO.File.Create(FP))
                {
                    await image.CopyToAsync(fs);
                    await fs.FlushAsync(CancellationToken.None);
                    await fs.DisposeAsync();
                }
                return $@"/images/{FN}";
            }
            else
            {
                return "No image has uploaded";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> AddUpdate(ProductVM productVM, string actionType, string token)
        {
            if (actionType == "add")
            {
                await db.Products.AddAsync(new Product(productVM.Name, productVM.ImageUrl));
                await db.SaveChangesAsync();
            }

            if (actionType == "edit")
            {
                Product product = new Product(productVM.Id, productVM.Name, productVM.ImageUrl);
                db.Products.Update(product);
                await db.SaveChangesAsync();
            }

            string trsWithTds = string.Empty;
            List<ProductVM> productVMs = db.Products.Select(x => new ProductVM(x.Id, x.Name, x.ImageUrl)).ToList();
            if (productVMs.Count > 0)
            {
                foreach (ProductVM aProductVM in productVMs)
                {
                    trsWithTds += "<tr><td>" + aProductVM.Name + "</td><td><img src=\"" + aProductVM.ImageUrl + "\"  /></td><td><button type=\"button\"  data-id=\"" + aProductVM.Id + "\" data-image=\"" + aProductVM.ImageUrl + "\" data-name=\"" + aProductVM.Name + "\" onclick=\"editProduct(this)\">Edit</button><button type=\"button\"  onclick=\"deleteProduct('" + token + "', " + aProductVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> Delete(int id, string token)
        {
            Product product = await db.Products.FindAsync(id);

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            string trsWithTds = string.Empty;
            List<ProductVM> productVMs = db.Products.Select(x => new ProductVM(x.Id, x.Name, x.ImageUrl)).ToList();
            if (productVMs.Count > 0)
            {
                foreach (ProductVM aProductVM in productVMs)
                {
                    trsWithTds += "<tr><td> " + aProductVM.Name + "</td><td ><img src=\"" + aProductVM.ImageUrl + " /></td><td><button type=\"button\"  data-id=\"" + aProductVM.Id + "\" data-image=\"" + aProductVM.ImageUrl + "\" data-name=\"" + aProductVM.Name + "\" onclick=\"editProduct(this)\">Edit</button><button type=\"button\"  onclick=\"deleteProduct('" + token + "', " + aProductVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }
    }
}
