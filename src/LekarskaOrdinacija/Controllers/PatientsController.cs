using LekarskaOrdinacija.Models;
using LekarskaOrdinacija.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LekarskaOrdinacija.Controllers;
[Authorize(Roles="Administrator,Doctor")]
public class PatientsController : Controller
{
    private readonly IRepository<Patient> _repo;
    public PatientsController(IRepository<Patient> repo)=>_repo=repo;
    public async Task<IActionResult> Index()=>View(await _repo.GetAllAsync());
    public async Task<IActionResult> Details(int id){var x=await _repo.GetByIdAsync(id);return x is null?NotFound():View(x);}
    [Authorize(Roles="Administrator,Doctor")] public IActionResult Create()=>View(new Patient());
    [HttpPost,ValidateAntiForgeryToken,Authorize(Roles="Administrator,Doctor")] public async Task<IActionResult> Create(Patient x){if(!ModelState.IsValid)return View(x);await _repo.AddAsync(x);await _repo.SaveAsync();return RedirectToAction(nameof(Index));}
    [Authorize(Roles="Administrator,Doctor")] public async Task<IActionResult> Edit(int id){var x=await _repo.GetByIdAsync(id);return x is null?NotFound():View(x);}
    [HttpPost,ValidateAntiForgeryToken,Authorize(Roles="Administrator,Doctor")] public async Task<IActionResult> Edit(Patient x){if(!ModelState.IsValid)return View(x);_repo.Update(x);await _repo.SaveAsync();return RedirectToAction(nameof(Index));}
    [Authorize(Roles="Administrator,Doctor")] public async Task<IActionResult> Delete(int id){var x=await _repo.GetByIdAsync(id);return x is null?NotFound():View(x);}
    [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken,Authorize(Roles="Administrator,Doctor")] public async Task<IActionResult> DeleteConfirmed(int id){var x=await _repo.GetByIdAsync(id);if(x!=null){_repo.Delete(x);await _repo.SaveAsync();}return RedirectToAction(nameof(Index));}
}