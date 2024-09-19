using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
                        return RedirectToAction(nameof(Login));
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
	}



}
