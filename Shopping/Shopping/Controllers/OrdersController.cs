using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Enums;
using Shopping.Helpers;
using Vereyon.Web;

namespace Shopping.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;
        private readonly INotyfService _notyf;
        private readonly IOrdersHelper _ordersHelper;

        public OrdersController(DataContext context,
            IFlashMessage flashMessage,
            INotyfService notyf, IOrdersHelper ordersHelper)
        {
            _context = context;
            _flashMessage = flashMessage;
            _notyf = notyf;
            _ordersHelper = ordersHelper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null)
            {
                return NotFound();
            }

            return View(sale);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dispatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus != OrderStatus.New)
            {
                //_notyf.Error("Order " + sale.OrderNo + " has been sent already.!");
                _flashMessage.Danger("Order Number: " + sale.OrderNo + " has been processed already.", "In-Process Status");
            }
            else
            {
                sale.OrderStatus = OrderStatus.InProccess;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();

                _flashMessage.Confirmation("Order status has been changed to In-Process successfuly.");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Send(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus != OrderStatus.InProccess)
            {
                //_flashMessage.Danger("Solo se pueden enviar pedidos que estén en estado 'despachado'.");
                _flashMessage.Danger("Only orders in In-Proccess status can be sent out.", "Invalid Order Status.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Sent;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                //_flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'enviado'.");
                _flashMessage.Confirmation("Order has been changed to sent status successfully.");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus != OrderStatus.Sent)
            {
                //_flashMessage.Danger("Solo se pueden confirmar pedidos que estén en estado 'enviado'.");
                _flashMessage.Danger("You can only confirm orders with sent status", "Invalid Order Status");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Confirmed;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                //_flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'confirmado'.");
                _flashMessage.Confirmation("Order has been confirmed successfuly.", "Confirmed Successfuly");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus == OrderStatus.Cancelled)
            {
                //_flashMessage.Danger("No se puede cancelar un pedido que esté en estado 'cancelado'.");
                _flashMessage.Danger("Order " + sale.OrderNo + " is already cancelled", "Invalid Status");
            }
            else
            {
                await _ordersHelper.CancelOrderAsync(sale.Id);
                _flashMessage.Confirmation("Order number " + sale.OrderNo + " has been cancelled successfully.", "Order Cancelled");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }


        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyOrders()
        {
            return View(await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .Where(s => s.User.UserName == User.Identity.Name)
                .ToListAsync());
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales
                .Include(s => s.User)
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(s => s.Id == id  && s.User.UserName == User.Identity.Name);

            if (sale == null)
            {
                _notyf.Error("You are not allowed to view this record");
                return NotFound();
               
            }

            return View(sale);

        }
    }
}