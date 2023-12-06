using MedSysApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedSysApi.Controllers
{
    public class WorkPlaceController : Controller
    {
        private readonly MedSysContext _context;
        public IActionResult Index(MedSysContext  context)
        {
           
            return View();
        }
    }
}
