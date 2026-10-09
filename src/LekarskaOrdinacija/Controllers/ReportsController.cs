using LekarskaOrdinacija.Data;
using LekarskaOrdinacija.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LekarskaOrdinacija.Controllers;
[Authorize(Roles="Administrator,Doctor")]
public class ReportsController : Controller
{
    private readonly ApplicationDbContext _db; public ReportsController(ApplicationDbContext db)=>_db=db;
    public async Task<IActionResult> Index(int? patientId,int? doctorId,DateTime? from,DateTime? to)
    {
        var q=_db.Appointments.AsNoTracking().Include(x=>x.Patient).Include(x=>x.Doctor).Include(x=>x.MedicalService).AsQueryable();
        if(patientId.HasValue)q=q.Where(x=>x.PatientId==patientId); if(doctorId.HasValue)q=q.Where(x=>x.DoctorId==doctorId);
        if(from.HasValue)q=q.Where(x=>x.Start>=from.Value); if(to.HasValue)q=q.Where(x=>x.Start<to.Value.Date.AddDays(1));
        ViewBag.ByDoctor=await q.GroupBy(x=>x.Doctor!.FullName).Select(g=>new{Doctor=g.Key,Scheduled=g.Count(),Completed=g.Count(x=>x.Status==AppointmentStatus.Completed)}).ToListAsync();
        ViewBag.ByService=await q.GroupBy(x=>x.MedicalService!.Name).Select(g=>new{Service=g.Key,Count=g.Count()}).OrderByDescending(x=>x.Count).ToListAsync();
        return View(await q.OrderByDescending(x=>x.Start).ToListAsync());
    }
}