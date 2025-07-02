using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Repositories.HoaLQ.Basic;
using SchoolMedical.Repositories.HoaLQ.Helper;
using SchoolMedical.Repositories.HoaLQ.Models;

namespace SchoolMedical.Repositories.HoaLQ
{
    public class HealthProfilesHoaLqRepository: GenericRepository<HealthProfilesHoaLq>
    {
        public HealthProfilesHoaLqRepository()
        {
            _context ??= new SU25_PRN222_SE1709_G1_SchoolMedicalContext();
        }

        public HealthProfilesHoaLqRepository(SU25_PRN222_SE1709_G1_SchoolMedicalContext context)
        {
            _context = context;
        }
        
        public async Task<PaginatedList<HealthProfilesHoaLq>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.HealthProfilesHoaLqs
                .Include(h => h.Student)
                .AsQueryable();
            var result = await PaginatedList<HealthProfilesHoaLq>.CreateAsync(query, pageNumber, pageSize);
            return result;
        }

        public new async Task<HealthProfilesHoaLq> GetByIdAsync(int id)
        {
            var healthProfile = await _context.HealthProfilesHoaLqs
                .AsNoTracking()
                .Include(h => h.Student)
                .FirstOrDefaultAsync(h => h.HealthProfileHoaLqid == id);
            return healthProfile ?? new HealthProfilesHoaLq();
        }

        public async Task<PaginatedList<HealthProfilesHoaLq>> SearchAsync(string studentName, string bloodType,bool? sex, int? minWeight, int? maxWeight, int? minHeight, int? maxHealth, int pageNumber, int pageSize)
        {
            var query = _context.HealthProfilesHoaLqs
                .Include(p => p.Student)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(studentName))
            {
                query = query.Where(p => p.Student.StudentFullName.Contains(studentName));
            }

            if (!string.IsNullOrWhiteSpace(bloodType))
            {
                query = query.Where(p => p.BloodType.Contains(bloodType));
            }

            if (sex.HasValue)
            {
                query = query.Where(p => p.Sex.Equals(sex));
            }

            if (minWeight.HasValue)
            {
                query = query.Where(p => p.Weight >= minWeight.Value);
            }

            if (maxWeight.HasValue)
            {
                query = query.Where(p => p.Weight <= maxWeight.Value);
            }

            if (minHeight.HasValue)
            {
                query = query.Where(p => p.Height >= minHeight.Value);
            }

            if (maxHealth.HasValue)
            {
                query = query.Where(p => p.Height <= maxHealth.Value);
            }

            var result = await PaginatedList<HealthProfilesHoaLq>.CreateAsync(query, pageNumber, pageSize);

            return result;
        }

        public async Task<List<HealthProfilesHoaLq>> FilterBySexAsync(bool? sex)
        {
                return await _context.HealthProfilesHoaLqs
                    .Include(h => h.Student)
                    .Where(h => h.Sex.Equals(sex))
                    .ToListAsync();
        }
        public new async Task<HealthProfilesHoaLq> CreateAsync(HealthProfilesHoaLq healthProfilesHoaLq)
        {
            var entity = _context.HealthProfilesHoaLqs.Add(healthProfilesHoaLq);
            await _context.SaveChangesAsync();
            var result = await _context.HealthProfilesHoaLqs
                .Include(h => h.Student) // <-- Dòng quan trọng để lấy dữ liệu Student
                .FirstOrDefaultAsync(h => h.HealthProfileHoaLqid == healthProfilesHoaLq.HealthProfileHoaLqid);

            return result;
        }

        public new async Task<HealthProfilesHoaLq> UpdateAsync(HealthProfilesHoaLq healthProfilesHoaLq)
        {
            var entity = _context.HealthProfilesHoaLqs.Update(healthProfilesHoaLq);
            await _context.SaveChangesAsync();
            var result = await _context.HealthProfilesHoaLqs
                .Include(h => h.Student) 
                .FirstOrDefaultAsync(h => h.HealthProfileHoaLqid == healthProfilesHoaLq.HealthProfileHoaLqid);
            return result;
        }
    }
}
