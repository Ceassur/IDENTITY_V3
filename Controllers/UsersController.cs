using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IDENTITY_V2.Models;
using Microsoft.AspNetCore.Identity;
using IDENTITY_V2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace IDENTITY_V2.Controllers;


public class UsersController : Controller
{
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UsersController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager){

            _userManager = userManager;
            _roleManager = roleManager;
        } 
        
        [Authorize(Roles = "ADMIN")]        
        public IActionResult Index(){
            
          /*  if(!User.IsInRole("ADMIN")){
                return RedirectToAction("Login","Account");
            }*/

            return View(_userManager.Users);

        }

  
        [HttpPost]
        public async Task<IActionResult> Delete(string id){
        
            var user = await _userManager.FindByIdAsync(id);

            if(user!=null){
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }


}
