using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Domain.Entities;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Contexts;
using VietCapital.Partner.F5Seconds.WebMvc.ModelView;

namespace VietCapital.Partner.F5Seconds.WebMvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGatewayHttpClientService _gatewayHttpClient;
        public ProductsController(ApplicationDbContext context, IGatewayHttpClientService gatewayHttpClient)
        {
            _gatewayHttpClient = gatewayHttpClient;
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Where(x => !string.IsNullOrEmpty(x.Image)).Take(20).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Code,ProductId,Name,Image,Price,Type,Size,Partner,BrandName,BrandLogo,Status,Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var product = await _context.Products.Include(x => x.CategoryProducts).SingleAsync(p => p.Id.Equals(id));
            if (product is null) return NotFound();
            var pGatewayDetail = await _gatewayHttpClient.DetailProduct(product.Code);
            if (!pGatewayDetail.Succeeded) return NotFound();
            if(pGatewayDetail.Data is null) return NotFound();
            if (product.Content is null) product.Content = pGatewayDetail.Data.productContent;
            if (product.Term is null) product.Term = pGatewayDetail.Data.productTerm;
            var productViewModel = new ProductEditView()
            {
                Product = product,
                CategoryList = new MultiSelectList(_context.Categories, nameof(Category.Id), nameof(Category.Name)),
                SelectedCategories = product.CategoryProducts == null ? null : product.CategoryProducts.Select(x => x.CategoryId),
                StoreList = pGatewayDetail.Data.storeList
            };
            return View(productViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditView pModelView)
        {
            if (id != pModelView.Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (pModelView.SelectedCategories is not null)
                    {
                        var newCategory = pModelView.SelectedCategories
                         .Select(c => new CategoryProduct
                         {
                             ProductId = id,
                             CategoryId = Convert.ToInt32(c)
                         });
                        _context.CategoryProducts.RemoveRange(await _context.CategoryProducts.Where(src => src.ProductId.Equals(id)).ToListAsync());
                        _context.CategoryProducts.AddRange(newCategory);
                    }
                    _context.Update(pModelView.Product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(pModelView.Product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pModelView);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
