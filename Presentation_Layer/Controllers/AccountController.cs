using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Presentation_Layer.Utilities;
using Presentation_Layer.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Presentation_Layer.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		private readonly DataContext dataContext;

        public AccountController(UserManager<ApplicationUser> userManager, DataContext dataContext,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
			this.dataContext = dataContext;
           this.signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

		[HttpPost]
		public IActionResult Register(RegisterViewModel registerViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(registerViewModel);

			}
			else
			{
				

              var FindUser = userManager.FindByEmailAsync(registerViewModel.Email).Result;
				if (FindUser is null)
				{
                    var user = new ApplicationUser
                    {
                        Firstname = registerViewModel.FirstName,
                        Lastname = registerViewModel.LastName,
                        UserName = registerViewModel.Username,
                        Email = registerViewModel.Email

                    };
                    var Result = userManager.CreateAsync(user, registerViewModel.Password).Result;

                    if (Result.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        foreach (var error in Result.Errors)
                        {

							ModelState.AddModelError(string.Empty, error.Description);
						}
                    }
                }
                else
				{
					ModelState.AddModelError("Email", "This email is already exist");

                }
				return View(registerViewModel);

			}
		}
		public IActionResult Login()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(loginViewModel);

			}
			else
			{
				var user =userManager.FindByEmailAsync(loginViewModel.Email).Result;
				if(user is not null)
				{
					if (userManager.CheckPasswordAsync(user, loginViewModel.Password).Result)
					{
						var result=signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false).Result;
						if (result.Succeeded)
						{
							return RedirectToAction(nameof(HomeController.Index), "Home");
						}
					}
					else
					{
						ModelState.AddModelError("Password", "Incorrect Email or Password");
					

					}
				}
				else
				{
					ModelState.AddModelError("Email", "This email Dosent exist");

				}
			}




			return View(loginViewModel);
		}


		public IActionResult SignOut()
		{
			signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}


		public IActionResult ForgetPassword()
		{
				return View();
		}


		[HttpPost]
		public IActionResult ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(forgetPasswordViewModel);

			}
			else
			{
				var user = userManager.FindByEmailAsync(forgetPasswordViewModel.Email).Result;
				if(user is not null)
				{
					//Generating Token
					var token=userManager.GeneratePasswordResetTokenAsync(user).Result;
					//Generating URl
					var url = Url.Action(nameof(ResetPassword),"Account",new {email=forgetPasswordViewModel.Email,Token=token},Request.Scheme);
					// Action-Controller - Route Values - Host&Protocol 
					var email = new Mail()
					{
						Subject = "Reset your Password",
						body=url,
						Recipent=forgetPasswordViewModel.Email
					};
					MailSettings.SendEmail(email);

					return RedirectToAction(nameof(CheckYourInbox));


				}
				else
				{
					ModelState.AddModelError("Email", "This email Dosent exist");
				}

			}
			return View(forgetPasswordViewModel);

		}

		public IActionResult ResetPassword(string email,string token)
		{
			var stringToken=token?.ToString()??string.Empty;
			var stringemail=email?.ToString()??string.Empty;
			if (email is null || token is null)
			{
				return BadRequest();
			}
			else
			{
				TempData["stringemail"] =email;
				TempData["stringToken"] =token;

			}

			return View();
		}
		[HttpPost]
		public IActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
		{
			resetPasswordViewModel.token = TempData["stringToken"]?.ToString();
			resetPasswordViewModel.email = TempData["stringemail"]?.ToString();
			if (!ModelState.IsValid)
			{
				return View(resetPasswordViewModel);
			}
			else
			{
				var user = userManager.FindByEmailAsync(resetPasswordViewModel.email).Result;
				if (user != null)
				{
					var result = userManager.ResetPasswordAsync(user, resetPasswordViewModel.token, resetPasswordViewModel.Password).Result;
					if (result.Succeeded) {
						
						return RedirectToAction(nameof(Login));
					}
					else
					{
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "User Not Found");
				}
				return View();
			}
		}

		public IActionResult GoogleLogin()
		{
			var prop = new AuthenticationProperties()
			{
				RedirectUri = Url.Action("GoogleResponse")
			};
			return Challenge(prop, GoogleDefaults.AuthenticationScheme);
		}

		public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
			var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claims => new
			{
				claims.Issuer,
				claims.OriginalIssuer,
				claims.Type,
				claims.Value
			});
			return RedirectToAction("Index","Home");
		}
		public IActionResult CheckYourInbox() => View();
		



	}



}
