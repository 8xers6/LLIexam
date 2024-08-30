using Microsoft.AspNetCore.Mvc;

namespace LLI.Controllers
{
    public class BarangayInformationController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add()
        {

        }
    }
}
