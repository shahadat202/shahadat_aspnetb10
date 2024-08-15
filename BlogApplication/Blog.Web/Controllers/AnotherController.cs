using Blog.Web.Models;
using Blog.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
namespace Blog.Controllers
{
    public class AnotherController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;

        public AnotherController([FromKeyedServices("another")]
            IEmailSender emailSender, ILogger<HomeController> logger)
        {
            _logger = logger;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
