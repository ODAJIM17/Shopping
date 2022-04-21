﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public HomeController(ILogger<HomeController> logger, INotyfService notyf, DataContext context, IUserHelper userHelper)
        {
            _logger = logger;
            _notyf = notyf;
            _context = context;
            _userHelper = userHelper;
        }

        //For insrtucttion https://codewithmukesh.com/blog/toast-notifications-in-aspnet-core/#Install_the_Package
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
         .Include(p => p.ProductImages)
         .Include(p => p.ProductCategories)
         .OrderBy(p => p.Name)
         .ToListAsync();
            List<ProductsHomeViewModel> productsHome = new() { new ProductsHomeViewModel() };
            int i = 1;
            foreach (Product product in products)
            {
                if (i == 1)
                {
                    productsHome.LastOrDefault().Product1 = product;
                }
                if (i == 2)
                {
                    productsHome.LastOrDefault().Product2 = product;
                }
                if (i == 3)
                {
                    productsHome.LastOrDefault().Product3 = product;
                }
                if (i == 4)
                {
                    productsHome.LastOrDefault().Product4 = product;
                    productsHome.Add(new ProductsHomeViewModel());
                    i = 0;
                }
                i++;
            }

            HomeViewModel model = new() { Products = productsHome };
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

            TemporalSale temporalSale = new()
            {
                Product = product,
                Quantity = 1,
                User = user
            };

            _context.TemporalSales.Add(temporalSale);
            await _context.SaveChangesAsync();
            _notyf.Success(temporalSale.Product.Name + " has been added successfully");
            return RedirectToAction(nameof(Index));
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