using AutoMapper;
using QApabilities.Students.API.Entities;
using QApabilities.Students.API.Repositories;
using QApabilities.Shared.DTOs;
using QApabilities.Shared.Common;

namespace QApabilities.Students.API.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMapper _mapper;

    public StudentService(IStudentRepository studentRepository, IMapper mapper)
    {
        _studentRepository = studentRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<StudentDto>> GetByIdAsync(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        
        if (student == null)
            return ApiResponse<StudentDto>.ErrorResult("Aluno não encontrado");

        var studentDto = _mapper.Map<StudentDto>(student);
        return ApiResponse<StudentDto>.SuccessResult(studentDto);
    }

    public async Task<ApiResponse<PaginatedResponse<StudentDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
    {
        var paginatedStudents = await _studentRepository.GetAllAsync(pageNumber, pageSize, searchTerm);
        var studentDtos = _mapper.Map<List<StudentDto>>(paginatedStudents.Items);
        
        var paginatedResponse = new PaginatedResponse<StudentDto>(
            studentDtos, 
            paginatedStudents.TotalCount, 
            paginatedStudents.PageNumber, 
            paginatedStudents.PageSize);

        return ApiResponse<PaginatedResponse<StudentDto>>.SuccessResult(paginatedResponse);
    }

    public async Task<ApiResponse<StudentDto>> CreateAsync(CreateStudentDto createStudentDto)
    {
        // Validações de negócio
        if (await _studentRepository.EmailExistsAsync(createStudentDto.Email))
            return ApiResponse<StudentDto>.ErrorResult("Email já está em uso");

        if (await _studentRepository.CpfExistsAsync(createStudentDto.Cpf))
            return ApiResponse<StudentDto>.ErrorResult("CPF já está em uso");

        // Validação de idade mínima (16 anos)
        var minimumAge = DateTime.UtcNow.AddYears(-16);
        if (createStudentDto.BirthDate > minimumAge)
            return ApiResponse<StudentDto>.ErrorResult("Aluno deve ter pelo menos 16 anos");

        var student = _mapper.Map<Student>(createStudentDto);
        var createdStudent = await _studentRepository.CreateAsync(student);
        
        var studentDto = _mapper.Map<StudentDto>(createdStudent);
        return ApiResponse<StudentDto>.SuccessResult(studentDto, "Aluno criado com sucesso");
    }

    public async Task<ApiResponse<StudentDto>> UpdateAsync(Guid id, UpdateStudentDto updateStudentDto)
    {
        var existingStudent = await _studentRepository.GetByIdAsync(id);
        if (existingStudent == null)
            return ApiResponse<StudentDto>.ErrorResult("Aluno não encontrado");

        // Verificar se o email já está em uso por outro aluno
        var studentWithEmail = await _studentRepository.GetByEmailAsync(updateStudentDto.Email);
        if (studentWithEmail != null && studentWithEmail.Id != id)
            return ApiResponse<StudentDto>.ErrorResult("Email já está em uso por outro aluno");

        // Atualizar apenas os campos permitidos
        existingStudent.Name = updateStudentDto.Name;
        existingStudent.Email = updateStudentDto.Email;
        existingStudent.Phone = updateStudentDto.Phone;
        existingStudent.Address = updateStudentDto.Address;

        var updatedStudent = await _studentRepository.UpdateAsync(existingStudent);
        var studentDto = _mapper.Map<StudentDto>(updatedStudent);
        
        return ApiResponse<StudentDto>.SuccessResult(studentDto, "Aluno atualizado com sucesso");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
    {
        var exists = await _studentRepository.ExistsAsync(id);
        if (!exists)
            return ApiResponse<bool>.ErrorResult("Aluno não encontrado");

        var deleted = await _studentRepository.DeleteAsync(id);
        return ApiResponse<bool>.SuccessResult(deleted, "Aluno removido com sucesso");
    }

    public async Task<ApiResponse<bool>> ExistsAsync(Guid id)
    {
        var exists = await _studentRepository.ExistsAsync(id);
        return ApiResponse<bool>.SuccessResult(exists);
    }
} 