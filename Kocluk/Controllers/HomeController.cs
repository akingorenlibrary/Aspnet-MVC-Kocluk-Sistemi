using Kocluk.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Kocluk.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var rolClaim = User?.FindFirst("Rol");

            if (rolClaim != null && rolClaim.Value != null && rolClaim.Value.Equals("admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
