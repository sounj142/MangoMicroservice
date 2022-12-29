using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index([FromQuery] string message)
        {
            return View((object)message);
        }
    }
}