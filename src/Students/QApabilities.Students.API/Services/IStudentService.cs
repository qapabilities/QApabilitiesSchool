using QApabilities.Shared.DTOs;
using QApabilities.Shared.Common;

namespace QApabilities.Students.API.Services;

public interface IStudentService
{
    Task<ApiResponse<StudentDto>> GetByIdAsync(Guid id);
    Task<ApiResponse<PaginatedResponse<StudentDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null);
    Task<ApiResponse<StudentDto>> CreateAsync(CreateStudentDto createStudentDto);
    Task<ApiResponse<StudentDto>> UpdateAsync(Guid id, UpdateStudentDto updateStudentDto);
    Task<ApiResponse<bool>> DeleteAsync(Guid id);
    Task<ApiResponse<bool>> ExistsAsync(Guid id);
} 