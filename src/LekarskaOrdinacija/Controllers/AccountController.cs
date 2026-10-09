using LekarskaOrdinacija.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace LekarskaOrdinacija.Controllers;
public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signIn;
    public AccountController(SignInManager<IdentityUser> signIn)=>_signIn=signIn;
    [HttpGet] public IActionResult Login()=>View();
    [HttpPost] public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if(!ModelState.IsValid)return View(vm);
        var result=await _signIn.PasswordSignInAsync(vm.Username,vm.Password,true,false);
        if(result.Succeeded)return RedirectToAction("Index","Home");
        ModelState.AddModelError("","Neispravni podaci za prijavu."); return View(vm);
    }
    [HttpPost] public async Task<IActionResult> Logout(){await _signIn.SignOutAsync();return RedirectToAction(nameof(Login));}
}