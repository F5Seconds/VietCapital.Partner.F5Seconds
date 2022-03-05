using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VietCapital.Partner.F5Seconds.Application.Interfaces;
using VietCapital.Partner.F5Seconds.Infrastructure.Persistence.Contexts;
using VietCapital.Partner.F5Seconds.WebMvc.Models;
using VietCapital.Partner.F5Seconds.WebMvc.ModelView;

namespace VietCapital.Partner.F5Seconds.WebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGatewayHttpClientService _gatewayHttpClient;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IGatewayHttpClientService gatewayHttpClient)
        {
            _logger = logger;
            _context = context;
            _gatewayHttpClient = gatewayHttpClient;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var p = from a in _context.Products
                    join b in _context.CategoryProducts on a.Id equals b.ProductId
                    join c in _context.Categories on b.CategoryId equals c.Id
                    select a;
            var products = await p.Where(x => !string.IsNullOrEmpty(x.Image)).Take(20).ToListAsync();
            return View(products);
        }

        [HttpGet("san-pham/chi-tiet/{id}")]
        public async Task<IActionResult> ProductDetail(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var product = await _context.Products.Include(cp => cp.CategoryProducts).ThenInclude(c => c.Category).SingleAsync(p => p.Code.Equals(id));
            if(product is null) return NotFound();
            var pGatewayDetail = await _gatewayHttpClient.DetailProduct(id);
            if (!pGatewayDetail.Succeeded) return NotFound();
            if (pGatewayDetail.Data is null) return NotFound();
            if (product.Content is null) product.Content = pGatewayDetail.Data.productContent;
            if (product.Term is null) product.Term = pGatewayDetail.Data.productTerm;
            return View(new ProductEditView()
            {
                Product = product,
                StoreList = pGatewayDetail.Data.storeList
            });
        }

        [HttpPost("transaction")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transaction(BuyVoucher buyVoucher)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var product = await _context.Products.SingleAsync(x => x.Code.Equals(buyVoucher.Code));
            if (product is null) return NotFound("Not found product");
            var payload = new Application.DTOs.Gateway.BuyVoucherPayload()
            {
                customerId = buyVoucher.Cif,
                propductId = buyVoucher.Code
            };
            var trans = await _gatewayHttpClient.BuyProduct(payload);
            if(!trans.Succeeded) return BadRequest(trans);
            foreach (var item in trans.Data)
            {
                bool expried = DateTime.TryParseExact(item.expiryDate, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime expiryDate);
                await _context.VoucherTransactions.AddAsync(new Domain.Entities.VoucherTransaction()
                {
                    Product = product,
                    CustomerId = buyVoucher.Cif,
                    CustomerPhone = payload.customerPhone,
                    ExpiryDate = expried? expiryDate : DateTime.Now,
                    ProductPrice = item.productPrice,
                    TransactionId = item.transactionId,
                    VoucherCode = item.voucherCode
                });
            }
            await _context.SaveChangesAsync();
            return RedirectToActionPermanent(nameof(ProblemDetails), new {id = buyVoucher.Code});
        }

        [HttpGet("danh-muc/{id}")]
        public async Task<IActionResult> Category(int? id)
        {
            if (id is null) return NotFound();
            var category = await _context.Categories
                .Include(c => c.CategoryProducts.Where(x => !string.IsNullOrEmpty(x.Product.Image)))
                .ThenInclude(a => a.Product)
                .SingleAsync(c => c.Id == id);
            return View(category);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
