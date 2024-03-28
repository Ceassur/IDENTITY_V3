using IDENTITY_V2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IDENTITY_V2.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(RoleManager<AppRole> roleManager){

            _roleManager = roleManager;
        }

public IActionResult Index(string searchtext, string Id, int pagenum, int rowcount)
{
    if(pagenum<1){
        pagenum = 1;
    }

    if(rowcount<1){
        rowcount = 5;
    }
    

        var roles1 = _roleManager.Roles.Where(x => 
        (string.IsNullOrEmpty(Id) || x.Id.Equals(Id))
        &&  
        (string.IsNullOrEmpty(searchtext) || x.Name.Equals(searchtext)))
            .Skip((pagenum - 1) * rowcount)
            .Take(rowcount);



            
        return View(roles1);
}

        public IActionResult Create(){
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppRole model){
            
            if(ModelState.IsValid){
                
                var result = await _roleManager.CreateAsync(model);

                if(result.Succeeded){

                    return RedirectToAction("Index");
                }

                foreach(var err in result.Errors){
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);
        }

    }
}