using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IDENTITY_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IDENTITY_V2.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace IDENTITY_V2.Controllers;

public class EczaneController : Controller{
     public IActionResult Index()
    {
        return View();
    }

   
}
