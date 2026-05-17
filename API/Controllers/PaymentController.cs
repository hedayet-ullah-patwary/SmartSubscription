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

        public IActionResult Index(int userId)
        {
            var payments = service.GetUserPayments(userId);
            return View(payments);
        }

        [HttpPost]
        public IActionResult Create(PaymentDTO payment)
        {
            var result = service.CreatePayment(payment);

            if (!result)
            {
                TempData["Error"] = "Payment failed";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Payment successful";

            return RedirectToAction("Index", new { userId = payment.UserId });
        }

        public IActionResult FailedPayments()
        {
            var data = service.GetFailedPayments();
            return View(data);
        }
    }
}