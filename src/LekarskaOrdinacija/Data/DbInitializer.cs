using LekarskaOrdinacija.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace LekarskaOrdinacija.Data;
public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db=services.GetRequiredService<ApplicationDbContext>();
        await db.Database.EnsureCreatedAsync();
        var roleManager=services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager=services.GetRequiredService<UserManager<IdentityUser>>();
        foreach(var role in new[]{"Administrator","Doctor","Nurse"}) if(!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole(role));
        await EnsureUser(userManager,"admin","Admin123!","Administrator");
        await EnsureUser(userManager,"lekar","Lekar123!","Doctor");
        await EnsureUser(userManager,"sestra","Sestra123!","Nurse");
        if(await db.Patients.AnyAsync()) return;
        var p1=new Patient{FirstName="Ana",LastName="Jovanović",Address="Bulevar 1, Beograd",DateOfBirth=new DateTime(1991,4,12),Phone="060111222",Email="ana@example.com"};
        var p2=new Patient{FirstName="Luka",LastName="Petrović",Address="Ulica 2, Beograd",DateOfBirth=DateTime.Today.AddYears(-8),Phone="060333444",Email="luka@example.com"};
        var d1=new Doctor{FullName="dr Marko Ilić",Specialization="Opšta medicina",Phone="011123456",Email="marko@ordinacija.rs"};
        var d2=new Doctor{FullName="dr Jelena Nikolić",Specialization="Pedijatrija",Phone="011123457",Email="jelena@ordinacija.rs"};
        var n1=new Nurse{FullName="Milica Savić",Phone="011222333",Email="milica@ordinacija.rs"};
        var s1=new MedicalService{Name="Opšti pregled",Price=3500m,DurationMinutes=30,Description="Standardni pregled lekara opšte prakse."};
        var s2=new MedicalService{Name="Pedijatrijski pregled",Price=4000m,DurationMinutes=30,Description="Pregled deteta kod pedijatra."};
        db.AddRange(p1,p2,d1,d2,n1,s1,s2); await db.SaveChangesAsync();
        foreach(var d in new[]{d1,d2}) foreach(var day in new[]{DayOfWeek.Monday,DayOfWeek.Tuesday,DayOfWeek.Wednesday,DayOfWeek.Thursday,DayOfWeek.Friday}) db.DoctorSchedules.Add(new DoctorSchedule{DoctorId=d.Id,DayOfWeek=day,StartTime=new TimeSpan(8,0,0),EndTime=new TimeSpan(16,0,0)});
        await db.SaveChangesAsync();
    }
    static async Task EnsureUser(UserManager<IdentityUser> um,string name,string pass,string role){var u=await um.FindByNameAsync(name);if(u==null){u=new IdentityUser{UserName=name,Email=$"{name}@ordinacija.local",EmailConfirmed=true};var r=await um.CreateAsync(u,pass);if(r.Succeeded)await um.AddToRoleAsync(u,role);}}
}