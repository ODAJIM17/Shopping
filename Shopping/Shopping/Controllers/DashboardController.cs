using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Helpers;

namespace Shopping.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public DashboardController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }
        public async Task<IActionResult> Index()
        {
            //Get user count
            int userCount = _context.Users.Count();
            if (userCount > 0)
            {
                ViewBag.UserCount = userCount;
            }
            else
            {
                ViewBag.User = 0;
            }

            //Get productcount
            int productCount = _context.Products.Count();
            if (productCount > 0)
            {
                ViewBag.ProductCount = productCount;
            }
            else
            {
                ViewBag.Product = 0;
            }

           return View( await _context.TemporalSales
                .Include(u => u.User)
                .Include(p => p.Product).ToListAsync());                     
        }
    }
}
