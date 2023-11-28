using Microsoft.AspNetCore.Mvc;

namespace MedSysApi.Controllers
{
    public class WorkPlaceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
