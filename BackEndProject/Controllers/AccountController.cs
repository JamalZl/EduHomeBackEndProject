using BackEndProject.Models;
using BackEndProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BackEndProject.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = registerVM.Email,
                Country = registerVM.Country
            };
            if (!registerVM.Terms)
            {
                ModelState.AddModelError("Terms", "Please check this field");
                return View();
            }
            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View();
                }
                return View();
            }

            await _userManager.AddToRoleAsync(appUser, "Member");
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            string link = Url.Action(nameof(VerifyEmail), "Account", new { email = appUser.Email, token }, Request.Scheme, Request.Host.ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("czeynalli00@gmail.com", "EduHome");
            mail.To.Add(new MailAddress(appUser.Email));

            mail.Subject = "Verify Email";
            string body = string.Empty;
            using (StreamReader reader = new StreamReader("wwwroot/assets/template/verifyemail.html"))
            {
                body = reader.ReadToEnd();
            }
            string aboutText = $"Welcome <strong>{appUser.Name+ " " + appUser.Surname}</strong> to our website ,please click the link down below to verify your account";

            body = body.Replace("{{link}}", link);
            mail.Body = body.Replace("{{aboutText}}", aboutText);
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential("czeynalli00@gmail.com", "born2win0078");
            smtp.Send(mail);
            TempData["Verify"] = true;
            //await _signInManager.SignInAsync(appUser, isPersistent: false);

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, true);
            TempData["Verified"] = true;

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(login.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            if (user.IsAdmin)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, login.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Your account has been blocked for 5 minutes");
                    return View();
                }
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();

            }

            return RedirectToAction("index", "home");
        }
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserEditVM editedUser = new UserEditVM
            {
                Name = user.Name,
                Surname = user.Surname,
                Country = user.Country,
                Username = user.UserName,
                Email = user.Email
            };
            return View(editedUser);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditVM editedUser)
        {

            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserEditVM userEdit = new UserEditVM
            {
                Username = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Surname=user.Surname,
                Country=user.Country
            };

            if (user.UserName != editedUser.Username && await _userManager.FindByNameAsync(editedUser.Username) != null)
            {
                ModelState.AddModelError("", $"{editedUser.Username} has already taken");
                return View(userEdit);
            }

            if (string.IsNullOrWhiteSpace(editedUser.CurrentPassword))
            {
                user.UserName = editedUser.Username;
                user.Email = editedUser.Email;
                user.Name = editedUser.Name;
                user.Surname = editedUser.Surname;
                user.Country = editedUser.Country;
                await _userManager.UpdateAsync(user);
                await _signInManager.SignInAsync(user,true);
            }
            else
            {
                user.UserName = editedUser.Username;
                user.Email = editedUser.Email;
                user.Name = editedUser.Name;
                user.Surname = editedUser.Surname;
                user.Country = editedUser.Country;

                IdentityResult result = await _userManager.ChangePasswordAsync(user, editedUser.CurrentPassword, editedUser.Password);

                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                    return View(userEdit);
                }
                await _signInManager.PasswordSignInAsync(user, editedUser.Password, true, true);

            }

            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");

        }
        //public IActionResult Show()
        //{
        //    return Content(User.Identity.IsAuthenticated.ToString());
        //}
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(AccountVM account)
        {
            AppUser user = await _userManager.FindByEmailAsync(account.AppUser.Email);
            if (user == null) return BadRequest();
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("czeynalli00@gmail.com", "EduHome");
            mail.To.Add(new MailAddress(user.Email));
            mail.Subject = "Reset Password";
            string body = string.Empty;
            using (StreamReader reader=new StreamReader("wwwroot/assets/template/ForgotPassword.html"))
            {
                body = reader.ReadToEnd();
            }
            string aboutText = $"Hello <strong>{user.Name +" " + user.Surname}</strong>";
            body = body.Replace("{{link}}", link);
            mail.Body = body.Replace("{{aboutText}}", aboutText);
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential("czeynalli00@gmail.com", "born2win0078");
            smtp.Send(mail);
            TempData["Forgot"] = true;
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            AppUser user = await _userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest();
            AccountVM model = new AccountVM
            {
                AppUser = user,
                Token = token
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(AccountVM account)
        {
            AppUser user = await _userManager.FindByEmailAsync(account.AppUser.Email);
            AccountVM model = new AccountVM
            {
                AppUser = user,
                Token = account.Token
            };
            if (!ModelState.IsValid) return View(model);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, account.Token, account.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            TempData["Reset"] = true;
            return RedirectToAction("Index", "Home");
        }
    }
}
