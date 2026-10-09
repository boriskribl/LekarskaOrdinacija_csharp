using LekarskaOrdinacija.Data;
using LekarskaOrdinacija.Models;
using LekarskaOrdinacija.Repositories;
using LekarskaOrdinacija.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace LekarskaOrdinacija.Controllers;
[Authorize(Roles="Administrator,Doctor,Nurse")]
public class AppointmentsController : Controller
{
    private readonly ApplicationDbContext _db; private readonly IAppointmentRepository _repo; private readonly IAppointmentService _service;
    public AppointmentsController(ApplicationDbContext db,IAppointmentRepository repo,IAppointmentService service){_db=db;_repo=repo;_service=service;}
    public async Task<IActionResult> Index()=>View(await _repo.GetDetailedAsync());
    [Authorize(Roles="Administrator,Doctor")] public async Task<IActionResult> Create(){await FillLists();return View(new Appointment{Start=DateTime.Today.AddDays(1).AddHours(9)});}
    [HttpPost,ValidateAntiForgeryToken,Authorize(Roles="Administrator,Doctor")]
    public async Task<IActionResult> Create(Appointment a){var v=await _service.ValidateAndPriceAsync(a);if(!v.ok)ModelState.AddModelError("",v.error!);if(!ModelState.IsValid){await FillLists();return View(a);}await _repo.AddAsync(a);await _repo.SaveAsync();return RedirectToAction(nameof(Index));}
    [HttpPost,Authorize(Roles="Administrator,Doctor")]
    public async Task<IActionResult> SetStatus(int id,AppointmentStatus status){var a=await _repo.GetByIdAsync(id);if(a==null)return NotFound();a.Status=status;_repo.Update(a);await _repo.SaveAsync();return RedirectToAction(nameof(Index));}
    [Authorize(Roles="Administrator,Doctor")] public async Task<IActionResult> Delete(int id){var a=await _repo.GetByIdAsync(id);if(a==null)return NotFound();_repo.Delete(a);await _repo.SaveAsync();return RedirectToAction(nameof(Index));}
    private async Task FillLists(){ViewBag.Patients=new SelectList(await _db.Patients.OrderBy(x=>x.LastName).ToListAsync(),"Id","FullName");ViewBag.Doctors=new SelectList(await _db.Doctors.OrderBy(x=>x.FullName).ToListAsync(),"Id","FullName");ViewBag.Services=new SelectList(await _db.MedicalServices.OrderBy(x=>x.Name).ToListAsync(),"Id","Name");}
}