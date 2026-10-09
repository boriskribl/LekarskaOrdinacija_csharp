using LekarskaOrdinacija.Models;
using LekarskaOrdinacija.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace LekarskaOrdinacija.Controllers;
[Authorize(Roles="Administrator,Doctor")]
public class MedicalServicesController : Controller
{
    private readonly IRepository<MedicalService> _repo;
    public MedicalServicesController(IRepository<MedicalService> repo)=>_repo=repo;
    public async Task<IActionResult> Index()=>View(await _repo.GetAllAsync());
    public async Task<IActionResult> Details(int id){var x=await _repo.GetByIdAsync(id);return x is null?NotFound():View(x);}
    [Authorize(Roles="Administrator")] public IActionResult Create()=>View(new MedicalService());
    [HttpPost,ValidateAntiForgeryToken,Authorize(Roles="Administrator")] public async Task<IActionResult> Create(MedicalService x){if(!ModelState.IsValid)return View(x);await _repo.AddAsync(x);await _repo.SaveAsync();return RedirectToAction(nameof(Index));}
    [Authorize(Roles="Administrator")] public async Task<IActionResult> Edit(int id){var x=await _repo.GetByIdAsync(id);return x is null?NotFound():View(x);}
    [HttpPost,ValidateAntiForgeryToken,Authorize(Roles="Administrator")] public async Task<IActionResult> Edit(MedicalService x){if(!ModelState.IsValid)return View(x);_repo.Update(x);await _repo.SaveAsync();return RedirectToAction(nameof(Index));}
    [Authorize(Roles="Administrator")] public async Task<IActionResult> Delete(int id){var x=await _repo.GetByIdAsync(id);return x is null?NotFound():View(x);}
    [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken,Authorize(Roles="Administrator")] public async Task<IActionResult> DeleteConfirmed(int id){var x=await _repo.GetByIdAsync(id);if(x!=null){_repo.Delete(x);await _repo.SaveAsync();}return RedirectToAction(nameof(Index));}
}