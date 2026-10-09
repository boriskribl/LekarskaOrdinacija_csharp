using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc;
namespace LekarskaOrdinacija.Controllers;
[Authorize] public class HomeController:Controller{public IActionResult Index()=>View(); public IActionResult Error()=>View();}