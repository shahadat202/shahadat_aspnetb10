using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IMemberShip _memberShip;
        public HomeController([FromKeyedServices("home")] IEmailSender emailSender,
            ILogger<HomeController> logger, IMemberShip memberShip)
        {
            _logger = logger;
            _emailSender = emailSender;
            _memberShip = memberShip;
        }

        public IActionResult Index()
        {
            var model = new IndexModel();
            model.Name = "Hello World!";
            return View(model);
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
                emailSender.SendEmail(model.Email, "Welcome", "Thank you");
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
