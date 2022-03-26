using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Models;
using System.Diagnostics;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, INotyfService notyf, DataContext context)
        {
            _logger = logger;
           _notyf = notyf;
          _context = context;
        }

        //For insrtucttion https://codewithmukesh.com/blog/toast-notifications-in-aspnet-core/#Install_the_Package
        public async Task<IActionResult> Index()
        {
           

            //_notyf.Success("Welcome to the Shopping Application " + User.Identity.Name);
            //_notyf.Success("Record saved successfully", 3);
            //_notyf.Information("Info Notification", 5);
            //_notyf.Error("OOPS! Something went wrong.");
            _notyf.Warning("This is a warning Notification",1);
            //_notyf.Custom("Custom Notification", 5, "whitesmoke", "fas fa-gear");
            //_notyf.Custom("Custom Notification", 5, "#B600FF", "fas fa-home");
            // return View();
            return View(await _context.Categories.ToListAsync());
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
            _notyf.Error("OOps!. Page not found",3);
            return View();
        }
    }
}