using LekarskaOrdinacija.Data;
using LekarskaOrdinacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace LekarskaOrdinacija.Controllers;
[Authorize(Roles="Administrator")]
public class DoctorSchedulesController : Controller
{
    private readonly ApplicationDbContext _db; public DoctorSchedulesController(ApplicationDbContext db)=>_db=db;
    public async Task<IActionResult> Index()=>View(await _db.DoctorSchedules.Include(x=>x.Doctor).OrderBy(x=>x.Doctor!.FullName).ThenBy(x=>x.DayOfWeek).ToListAsync());
    public async Task<IActionResult> Create(){ViewBag.Doctors=new SelectList(await _db.Doctors.ToListAsync(),"Id","FullName");return View(new DoctorSchedule{StartTime=new TimeSpan(8,0,0),EndTime=new TimeSpan(16,0,0)});}
    [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(DoctorSchedule x){if(x.EndTime<=x.StartTime)ModelState.AddModelError("EndTime","Kraj radnog vremena mora biti posle početka.");if(!ModelState.IsValid){ViewBag.Doctors=new SelectList(await _db.Doctors.ToListAsync(),"Id","FullName");return View(x);}_db.Add(x);await _db.SaveChangesAsync();return RedirectToAction(nameof(Index));}
    [HttpPost] public async Task<IActionResult> Delete(int id){var x=await _db.DoctorSchedules.FindAsync(id);if(x!=null){_db.Remove(x);await _db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
}