using Microsoft.EntityFrameworkCore;
using QApabilities.Students.API.Data;
using QApabilities.Students.API.Entities;
using QApabilities.Shared.Common;

namespace QApabilities.Students.API.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly StudentsDbContext _context;

    public StudentRepository(StudentsDbContext context)
    {
        _context = context;
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        return await _context.Students
            .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<Student?> GetByEmailAsync(string email)
    {
        return await _context.Students
            .FirstOrDefaultAsync(s => s.Email == email && s.IsActive);
    }

    public async Task<Student?> GetByCpfAsync(string cpf)
    {
        return await _context.Students
            .FirstOrDefaultAsync(s => s.Cpf == cpf && s.IsActive);
    }

    public async Task<PaginatedResponse<Student>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm = null)
    {
        var query = _context.Students.Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s => 
                s.Name.Contains(searchTerm) || 
                s.Email.Contains(searchTerm) || 
                s.Cpf.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var students = await query
            .OrderBy(s => s.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponse<Student>(students, totalCount, pageNumber, pageSize);
    }

    public async Task<Student> CreateAsync(Student student)
    {
        student.Id = Guid.NewGuid();
        student.CreatedAt = DateTime.UtcNow;
        student.IsActive = true;

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return student;
    }

    public async Task<Student> UpdateAsync(Student student)
    {
        student.UpdatedAt = DateTime.UtcNow;
        
        _context.Students.Update(student);
        await _context.SaveChangesAsync();

        return student;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var student = await GetByIdAsync(id);
        if (student == null)
            return false;

        student.IsActive = false;
        student.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Students
            .AnyAsync(s => s.Id == id && s.IsActive);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Students
            .AnyAsync(s => s.Email == email && s.IsActive);
    }

    public async Task<bool> CpfExistsAsync(string cpf)
    {
        return await _context.Students
            .AnyAsync(s => s.Cpf == cpf && s.IsActive);
    }
} 