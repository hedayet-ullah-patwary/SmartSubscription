using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService service;

        public PaymentController(PaymentService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) 
                return RedirectToAction("Login", "Auth");

            var payments = service.GetUserPayments(userId.Value);
            return View(payments);
        }

        [HttpPost]
        public IActionResult Create(PaymentDTO payment)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) 
                return RedirectToAction("Login", "Auth");

            payment.UserId = userId.Value;

            var result = service.CreatePayment(payment);

            if (!result)
            {
                TempData["Error"] = "Payment failed";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Payment successful";
            return RedirectToAction("Index");
        }
    }
}