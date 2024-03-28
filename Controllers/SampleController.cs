using IDENTITY_V2.Models;
using IDENTITY_V3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IDENTITY_V2.Controllers
{
    public class SampleController : Controller
    {      
        public IActionResult Index(){
            
            return View();
        }

        [HttpGet]
        public IActionResult Create(Sample model){
          
          return View();
        }

    }
}