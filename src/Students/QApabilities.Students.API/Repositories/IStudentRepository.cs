using QApabilities.Students.API.Entities;
using QApabilities.Shared.Common;

namespace QApabilities.Students.API.Repositories;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(Guid id);
    Task<Student?> GetByEmailAsync(string email);
    Task<Student?> GetByCpfAsync(string cpf);
    Task<PaginatedResponse<Student>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null);
    Task<Student> CreateAsync(Student student);
    Task<Student> UpdateAsync(Student student);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> CpfExistsAsync(string cpf);
} 