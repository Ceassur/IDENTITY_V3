using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IDENTITY_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IDENTITY_V2.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace IDENTITY_V2.Controllers;

public class ForgotPasswordController : Controller{

     public IActionResult Index()
    {
        return View();
    }
   
   [HttpGet]
    public IActionResult UpdatePassword(string userId, string token, string password, string passwordconfirm)
    {
       // return "Basarili";
        return RedirectToAction("Index","Home");
    }
   
}
