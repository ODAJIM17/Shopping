using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Common;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Helpers;
using Shopping.Models;
using Shopping.ViewModels;
using System.Diagnostics;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IOrdersHelper _ordersHelper;

        public HomeController(ILogger<HomeController> logger, 
            INotyfService notyf, 
            DataContext context, 
            IUserHelper userHelper, 
            IOrdersHelper ordersHelper)
        {
            _logger = logger;
            _notyf = notyf;
            _context = context;
            _userHelper = userHelper;
            _ordersHelper = ordersHelper;
        }

        // for instructions about toast go to this link https://codewithmukesh.com/blog/toast-notifications-in-aspnet-core/#Install_the_Package
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "NameDesc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "PriceDesc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;


            IQueryable<Product> query = _context.Products
              .Include(p => p.ProductImages)
              .Include(p => p.ProductCategories)
              .ThenInclude(pc => pc.Category);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => (p.Name.ToLower().Contains(searchString.ToLower()) ||
                                            p.ProductCategories.Any(pc => pc.Category.Name.ToLower()
                                            .Contains(searchString.ToLower()))) &&
                                            p.Stock > 0);
            }
            else
            {
                query = query.Where(p => p.Stock > 0);
            }


            switch (sortOrder)
            {
                case "NameDesc":
                    query = query.OrderByDescending(p => p.Name);
                    break;
                case "Price":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "PriceDesc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            int pageSize = 8;

            HomeViewModel model = new() 
            {
                Products = await PaginatedList<Product>.CreateAsync(query, pageNumber ?? 1, pageSize),
                Categories = await _context.Categories.ToListAsync(),
            };

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                model.Quantity = await _context.TemporalSales
                    .Where(ts => ts.User.Id == user.Id)
                    .SumAsync(ts => ts.Quantity);
            }

          
            //Capture the qty
            ViewBag.Cart = model.Quantity;

            return View(model);



        }


        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                _notyf.Information("Please login to place an order.");
                return RedirectToAction("Login", "Account");
            }

            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            //Check if the item is already in the shopping cart. If it is just add 1 to the Qty
            var orderDetailTemp = await _context.TemporalSales
                .Where(ts => ts.User.Id == user.Id && ts.Product.Id == id)
                .FirstOrDefaultAsync();

            if (orderDetailTemp != null)
            {
                orderDetailTemp.Quantity += 1;
                _context.TemporalSales.Update(orderDetailTemp);
                _notyf.Success(orderDetailTemp.Product.Name + " has been added successfully");
            }
            else
            {
                 TemporalSale temporalSale = new()
                {
                    Product = product,
                    Quantity = 1,
                    User = user
                };

                _context.TemporalSales.Add(temporalSale);
                _notyf.Success(temporalSale.Product.Name + " has been added successfully");
               
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            string categories = string.Empty;
            foreach (ProductCategory category in product.ProductCategories)
            {
                categories += $"{category.Category.Name}, ";
            }
            categories = categories.Substring(0, categories.Length - 2);

            AddProductToCartViewModel model = new()
            {
                Categories = categories,
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ProductImages = product.ProductImages,
                Quantity = 1,
                Stock = product.Stock,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(AddProductToCartViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            Product product = await _context.Products.FindAsync(model.Id);
            if (product == null)
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            //Check if the item is already in the shopping cart. If it is just add 1 to the Qty
            var orderDetailTemp = await _context.TemporalSales
                .Where(ts => ts.User.Id == user.Id && ts.Product.Id == model.Id)
                .FirstOrDefaultAsync();

            if (orderDetailTemp != null)
            {
                orderDetailTemp.Quantity += model.Quantity; 
                _context.TemporalSales.Update(orderDetailTemp);
                _notyf.Success(orderDetailTemp.Product.Name + " has been added successfully");

            }
            else
            {
                // client is adding a new item
                TemporalSale temporalSale = new()
                {
                    Product = product,
                    Quantity = model.Quantity,
                    Remarks = model.Remarks,
                    User = user                
            };

                _context.TemporalSales.Add(temporalSale);
                _notyf.Success(temporalSale.Product.Name + " has been added successfully");

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        [Authorize]
        public async Task<IActionResult> ShowCart()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            List<TemporalSale> temporalSales = await _context.TemporalSales
                .Include(ts => ts.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            ShowCartViewModel model = new()
            {
                User = user,
                TemporalSales = temporalSales,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowCart(ShowCartViewModel model)
        {
           

           User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            model.User = user;
            model.TemporalSales = await _context.TemporalSales
                .Include(ts => ts.Product)
                .ThenInclude(p => p.ProductImages)
                .Where(ts => ts.User.Id == user.Id)
                .ToListAsync();

            Response response = await _ordersHelper.ProcessOrderAsync(model);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(OrderSuccess));
            }

            ModelState.AddModelError(string.Empty, response.Message);
            return View(model);
        }

        public async Task<IActionResult> DecreaseQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }

            if (temporalSale.Quantity > 1)
            {
                temporalSale.Quantity--;
                _context.TemporalSales.Update(temporalSale);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> IncreaseQuantity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }

            temporalSale.Quantity++;
            _context.TemporalSales.Update(temporalSale);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ShowCart));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }

            _context.TemporalSales.Remove(temporalSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowCart));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
            if (temporalSale == null)
            {
                return NotFound();
            }

            EditTemporalSaleViewModel model = new()
            {
                Id = temporalSale.Id,
                Quantity = temporalSale.Quantity,
                Remarks = temporalSale.Remarks,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditTemporalSaleViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TemporalSale temporalSale = await _context.TemporalSales.FindAsync(id);
                    temporalSale.Quantity = model.Quantity;
                    temporalSale.Remarks = model.Remarks;
                    _context.Update(temporalSale);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                    return View(model);
                }

                return RedirectToAction(nameof(ShowCart));
            }

            return View(model);
        }


        [Authorize]
        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            // var br =  HttpContext.Request.Browser.Browser;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            _notyf.Error("OOps!. Page not found", 3);
            return View();
        }
    }
}