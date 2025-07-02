using SchoolMedical.Repositories.HoaLQ;
using SchoolMedical.Repositories.HoaLQ.Helper;
using SchoolMedical.Repositories.HoaLQ.Models;

namespace SchoolMedical.Services.HoaLQ;

public interface IHealthProfilesHoaLqService
{
    Task<PaginatedList<HealthProfilesHoaLq>> GetAllAsync(int pageNumber, int pageSize);
    Task<HealthProfilesHoaLq> GetByIdAsync(int id);

    Task<PaginatedList<HealthProfilesHoaLq>> SearchAsync(string studentName, string bloodType, bool? sex,
        int? minWeight, int? maxWeight, int? minHeight, int? maxHealth, int pageNumber, int pageSize);   Task<List<HealthProfilesHoaLq>> FilterBySexAsync(bool? sex);
    Task<HealthProfilesHoaLq> CreateAsync(HealthProfilesHoaLq healthProfilesHoaLq);
    Task<HealthProfilesHoaLq> UpdateAsync(HealthProfilesHoaLq healthProfilesHoaLq);
    Task<bool> RemoveAsync(int id);
}
public class HealthProfilesHoaLqService: IHealthProfilesHoaLqService
{
    private readonly IUnitOfWork _unitOfWork;
    public HealthProfilesHoaLqService()
    {
        _unitOfWork  ??= new UnitOfWork();
    }

    public async Task<PaginatedList<HealthProfilesHoaLq>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _unitOfWork.HealthProfilesHoaLqRepository.GetAllAsync(pageNumber,pageSize);
    }
    
    public async Task<HealthProfilesHoaLq> GetByIdAsync(int id)
    {
        return await _unitOfWork.HealthProfilesHoaLqRepository.GetByIdAsync(id);
    }
    
    
    
    public async Task<PaginatedList<HealthProfilesHoaLq>> SearchAsync(string studentName, string bloodType, bool? sex, int? minWeight, int? maxWeight, int? minHeight, int? maxHealth, int pageNumber, int pageSize)
    {
        return await _unitOfWork.HealthProfilesHoaLqRepository.SearchAsync(studentName, bloodType,sex, minWeight, maxWeight, minHeight, maxHealth, pageNumber, pageSize);
    }

    public async Task<List<HealthProfilesHoaLq>> FilterBySexAsync(bool? sex)
    {
        return await _unitOfWork.HealthProfilesHoaLqRepository.FilterBySexAsync(sex);
    }
    
    public async Task<HealthProfilesHoaLq> CreateAsync(HealthProfilesHoaLq healthProfilesHoaLq)
    {
        var createdProfile = await _unitOfWork.HealthProfilesHoaLqRepository.CreateAsync(healthProfilesHoaLq);
        Console.WriteLine(createdProfile);
        return createdProfile;
    }
    
    public async Task<HealthProfilesHoaLq> UpdateAsync(HealthProfilesHoaLq healthProfilesHoaLq)
    {
        var updatedProfile = await _unitOfWork.HealthProfilesHoaLqRepository.UpdateAsync(healthProfilesHoaLq);
        return updatedProfile;
    }
    
    public async Task<bool> RemoveAsync(int id)
    {
        var removedProfile = await _unitOfWork.HealthProfilesHoaLqRepository.GetByIdAsync(id);
        return await _unitOfWork.HealthProfilesHoaLqRepository.RemoveAsync(removedProfile);
    }
}