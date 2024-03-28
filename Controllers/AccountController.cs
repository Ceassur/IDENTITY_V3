using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IDENTITY_V2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IDENTITY_V2.ViewModels;
using Microsoft.EntityFrameworkCore;
using AspNetCore.ReCaptcha;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace IDENTITY_V2.Controllers;

public class AccountController : Controller
{
    public class reCaptchaResponse
    {
        public bool Success { get; set; }
        public string[] ErrorCodes { get; set; }
    }

    private UserManager<AppUser> _userManager;
    private RoleManager<AppRole> _roleManager;
    private SignInManager<AppUser> _signInManager;
    private IEmailSender _emailSender;

    public AccountController(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    IEmailSender emailSender
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var errorText = "";
        if (ModelState.IsValid)

        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"secret", "6LeWCn8pAAAAAAMIPsJmqUUqDxNzAHzbj0nWfwd9" },
                    {"response", model.ReCaptchaResponse }
                });

            var client = new HttpClient();
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);


            if (response.IsSuccessStatusCode)
            {
                var result2 = await response.Content.ReadFromJsonAsync<reCaptchaResponse>();
                if (!result2.Success)
                {
                    errorText = "Doğrulama Başarısız!";
                    return StatusCode(500, errorText);
                }

            }
            else
            {
                errorText = "Doğrulama başarısız!";
                return StatusCode(500, errorText);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                await _signInManager.SignOutAsync();

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("", "Hesabınızı onaylayınız.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                    await _userManager.SetLockoutEndDateAsync(user, null);

                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                    var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                    ModelState.AddModelError("", $"Hesabınız kitlendi, Lütfen {timeLeft.Minutes} dakika sonra deneyiniz");
                }
                else
                {
                    ModelState.AddModelError("", "parolanız hatalı");
                }
            }
            else
            {
                ModelState.AddModelError("", "bu email adresiyle bir hesap bulunamadı");
            }
        }
        else
        {
            errorText += "Eksik veri girişi yaptınız!";
            return StatusCode(500, errorText);
        }

        return StatusCode(200, "Veri girişi başarılı");
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateViewModel model)
    {

        var errorText = "";

        if (ModelState.IsValid)
        {

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"secret", "6LeWCn8pAAAAAAMIPsJmqUUqDxNzAHzbj0nWfwd9" },
                    {"response", model.ReCaptchaResponse }
                });

            var client = new HttpClient();
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);


            if (response.IsSuccessStatusCode)
            {
                var result2 = await response.Content.ReadFromJsonAsync<reCaptchaResponse>();
            }
            else
            {
                errorText = "Doğrulama başarısız!";
                return StatusCode(500, errorText);
            }


            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                IsActive = true
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                if (model.SelectedRoles != null)
                {
                    await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                }
                else
                {

                }

                //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //var url = Url.Action("ConfirmEmail","Account", new {user.Id,token});

                //await _emailSender.SendEmailAsync(user.Email,"Hesap Onayı","http://localhost:5070/"+url);

                //TempData["message"] = "Hesabınızı onaylmak için mail adresinizi kontrol edin!";
                // return RedirectToAction("Login", "Account");
            }

            foreach (IdentityError err in result.Errors)
            {
                errorText += err + "-";
                return StatusCode(500, errorText);
            }

        }
        else
        {
            errorText += "Eksik veri girişi yaptınız!";
            return StatusCode(500, errorText);
        }

        return StatusCode(200, "Veri girişi başarılı");
    }

    public async Task<IActionResult> ConfirmEmail(string Id, string token)
    {

        if (Id == null || token == null)
        {
            TempData["message"] = "Geçersiz token bilgisi!";
            return View();
        }

        var user = await _userManager.FindByIdAsync(Id);

        if (user != null)
        {

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                TempData["message"] = "Hesabınız Onaylandı!";
                return View();
            }

        }
        else
        {
            TempData["message"] = "Kullanıcı bulunamadı!";
        }


        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }




}


