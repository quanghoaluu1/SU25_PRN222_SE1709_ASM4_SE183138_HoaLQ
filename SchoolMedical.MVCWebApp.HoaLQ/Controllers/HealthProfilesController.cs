using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Repositories.HoaLQ.Models;
using SchoolMedical.Services.HoaLQ;

namespace SchoolMedical.MVCWebApp.HoaLQ.Controllers
{
    public class HealthProfilesController : Controller
    {
        // private readonly SU25_PRN222_SE1709_G1_SchoolMedicalContext _context;
        private readonly IServiceProviders _serviceProviders;

        public HealthProfilesController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        // GET: HealthProfile
        public async Task<IActionResult> Index()
        {
           
            var healthProfiles = await _serviceProviders.HealthProfilesHoaLqService.GetAllAsync();
            return View(healthProfiles);
        }

        // GET: HealthProfile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthProfilesHoaLq = await _serviceProviders.HealthProfilesHoaLqService.GetByIdAsync(id.Value);
            
            if (healthProfilesHoaLq == null)
            {
                return NotFound();
            }

            return View(healthProfilesHoaLq);
        }

        //GET: HealthProfile/Create
        public async Task<IActionResult> Create()
        {
            var studentsQueryable = (await _serviceProviders.StudentHoaLqService.GetAllAsync()).AsQueryable();
            var students = studentsQueryable.Select(s => new
            {
                Id = s.StudentsHoaLqid,
                StudentInfo = $"{(s.Class.ClassName)} - {s.StudentFullName} - {s.StudentCode}"
            });
            ViewData["StudentList"] = new SelectList(students, "Id", "StudentInfo");
            
            var healthProfilesHoaLq = new HealthProfilesHoaLq()
            {
                Weight = 0,
                Height = 0,
                Sight = 0,
                Hearing = 0,
                BloodPressure = 0,
                Allergy = string.Empty,
                ChronicDisease = string.Empty,
                MedicalHistory = string.Empty,
                CurrentMedical = string.Empty,
                BloodType = "O+",
            };
            return View(healthProfilesHoaLq);
        }
        
        // POST: HealthProfile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HealthProfilesHoaLq healthProfilesHoaLq)
        {
            try
            {
                if (ModelState.IsValid)
                {
                        await _serviceProviders.HealthProfilesHoaLqService.CreateAsync(healthProfilesHoaLq);

                        Console.WriteLine("Create Successfully");
                        return RedirectToAction(nameof(Index));

                }
                var studentsQueryable = (await _serviceProviders.StudentHoaLqService.GetAllAsync()).AsQueryable();

                var students = studentsQueryable.Select(s => new
                {
                    Id = s.StudentsHoaLqid,
                    StudentInfo = $"{(s.Class.ClassName)} - {s.StudentFullName} - {s.StudentCode}"
                });
                ViewData["StudentList"] = new SelectList(students, "Id", "StudentInfo");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View(healthProfilesHoaLq);
        }
        //
        // // GET: HealthProfile/Edit/5
        // public async Task<IActionResult> Edit(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var healthProfilesHoaLq = await _context.HealthProfilesHoaLqs.FindAsync(id);
        //     if (healthProfilesHoaLq == null)
        //     {
        //         return NotFound();
        //     }
        //     ViewData["StudentId"] = new SelectList(_context.StudentsHoaLqs, "StudentsHoaLqid", "StudentsHoaLqid", healthProfilesHoaLq.StudentId);
        //     return View(healthProfilesHoaLq);
        // }
        //
        // // POST: HealthProfile/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id, [Bind("HealthProfileHoaLqid,StudentId,Weight,Height,Sight,Hearing,BloodPressure,Allergy,ChronicDisease,MedicalHistory,CurrentMedical,BloodType,Sex,DateOfBirth")] HealthProfilesHoaLq healthProfilesHoaLq)
        // {
        //     if (id != healthProfilesHoaLq.HealthProfileHoaLqid)
        //     {
        //         return NotFound();
        //     }
        //
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(healthProfilesHoaLq);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!HealthProfilesHoaLqExists(healthProfilesHoaLq.HealthProfileHoaLqid))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["StudentId"] = new SelectList(_context.StudentsHoaLqs, "StudentsHoaLqid", "StudentsHoaLqid", healthProfilesHoaLq.StudentId);
        //     return View(healthProfilesHoaLq);
        // }
        //
        // // GET: HealthProfile/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     var healthProfilesHoaLq = await _context.HealthProfilesHoaLqs
        //         .Include(h => h.Student)
        //         .FirstOrDefaultAsync(m => m.HealthProfileHoaLqid == id);
        //     if (healthProfilesHoaLq == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     return View(healthProfilesHoaLq);
        // }
        //
        // // POST: HealthProfile/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var healthProfilesHoaLq = await _context.HealthProfilesHoaLqs.FindAsync(id);
        //     if (healthProfilesHoaLq != null)
        //     {
        //         _context.HealthProfilesHoaLqs.Remove(healthProfilesHoaLq);
        //     }
        //
        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }
        //
        // private bool HealthProfilesHoaLqExists(int id)
        // {
        //     return _context.HealthProfilesHoaLqs.Any(e => e.HealthProfileHoaLqid == id);
        // }
    }
}
