using DevSkill.Inventory.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DevSkill.Inventory.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailsender;

        public HomeController(ILogger<HomeController> logger, 
            IEmailSender emailsender)
        {
            _logger = logger;
            _emailsender = emailsender;
        }

        public IActionResult Index()
        {
            // This log(ILogger) work after run application.
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Test()
        {
            var model = new TestModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Test(TestModel model)
        {
            if (ModelState.IsValid)
            {
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmail(model.Email, "Welcome", "Thank You!");
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
