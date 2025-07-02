using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Repositories.HoaLQ.Models;
using SchoolMedical.Services.HoaLQ;

namespace SchoolMedical.MVCWebApp.HoaLQ.Controllers
{
    [Authorize]
    public class HealthProfilesController : Controller
    {
        // private readonly SU25_PRN222_SE1709_G1_SchoolMedicalContext _context;
        private readonly IServiceProviders _serviceProviders;

        public HealthProfilesController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        // GET: HealthProfile
        public async Task<IActionResult> Index(string studentName, string bloodType, int? minWeight, int? maxWeight, int? minHeight, int? maxHealth, int? pageNumber)
        {
            ViewData["StudentName"] = studentName;
            ViewData["BloodType"] = bloodType;
            ViewData["MinWeight"] = minWeight;
            ViewData["MaxWeight"] = maxWeight;
            ViewData["MinHeight"] = minHeight;
            ViewData["MaxHeight"] = maxHealth;
            var pageSize = 10;
            var paginatedResult = await _serviceProviders.HealthProfilesHoaLqService.SearchAsync(studentName, bloodType, minWeight, maxWeight, minHeight, maxHealth, pageNumber ?? 1, pageSize);
            return View(paginatedResult);
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
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
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
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var healthProfilesHoaLq = await _serviceProviders.HealthProfilesHoaLqService.GetByIdAsync(id);
            if (healthProfilesHoaLq is null)
            {
                return NotFound();
            }
            var studentsQueryable = (await _serviceProviders.StudentHoaLqService.GetAllAsync()).AsQueryable();

            var students = studentsQueryable.Select(s => new
            {
                Id = s.StudentsHoaLqid,
                StudentInfo = $"{(s.Class.ClassName)} - {s.StudentFullName} - {s.StudentCode}"
            });
            ViewData["StudentList"] = new SelectList(students, "Id", "StudentInfo");
            return View(healthProfilesHoaLq);
        }
        //
        // // POST: HealthProfile/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Edit(int id, HealthProfilesHoaLq healthProfilesHoaLq)
        {
            if (id != healthProfilesHoaLq.HealthProfileHoaLqid)
            {
                return NotFound();
            }
        
            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceProviders.HealthProfilesHoaLqService.UpdateAsync(healthProfilesHoaLq);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HealthProfilesHoaLqExists(healthProfilesHoaLq.HealthProfileHoaLqid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var studentsQueryable = (await _serviceProviders.StudentHoaLqService.GetAllAsync()).AsQueryable();

            var students = studentsQueryable.Select(s => new
            {
                Id = s.StudentsHoaLqid,
                StudentInfo = $"{(s.Class.ClassName)} - {s.StudentFullName} - {s.StudentCode}"
            });
            ViewData["StudentList"] = new SelectList(students, "Id", "StudentInfo");
            return View(healthProfilesHoaLq);
        }
        
        // // GET: HealthProfile/Delete/5
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthProfilesHoaLq = await _serviceProviders.HealthProfilesHoaLqService.GetByIdAsync(id);
            if (healthProfilesHoaLq == null)
            {
                return NotFound();
            }
        
            return View(healthProfilesHoaLq);
        }
        
        // // POST: HealthProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = false;
            var healthProfilesHoaLq = await _serviceProviders.HealthProfilesHoaLqService.GetByIdAsync(id);
            if (healthProfilesHoaLq != null)
            {
               result = await _serviceProviders.HealthProfilesHoaLqService.RemoveAsync(id);
            }
            if (!result)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        
        private bool HealthProfilesHoaLqExists(int id)
        {
            return _serviceProviders.HealthProfilesHoaLqService.GetByIdAsync(id) is not null;
        }
    }
}
