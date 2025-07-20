using Microsoft.AspNetCore.Mvc;
using QApabilities.Students.API.Services;
using QApabilities.Shared.DTOs;
using QApabilities.Shared.Common;

namespace QApabilities.Students.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// Obtém um aluno pelo ID
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>Dados do aluno</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), 404)]
    public async Task<ActionResult<ApiResponse<StudentDto>>> GetById(Guid id)
    {
        var result = await _studentService.GetByIdAsync(id);
        
        if (!result.Success)
            return NotFound(result);
            
        return Ok(result);
    }

    /// <summary>
    /// Lista todos os alunos com paginação
    /// </summary>
    /// <param name="pageNumber">Número da página (padrão: 1)</param>
    /// <param name="pageSize">Tamanho da página (padrão: 10)</param>
    /// <param name="searchTerm">Termo de busca opcional</param>
    /// <returns>Lista paginada de alunos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<StudentDto>>), 200)]
    public async Task<ActionResult<ApiResponse<PaginatedResponse<StudentDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
    {
        var result = await _studentService.GetAllAsync(pageNumber, pageSize, searchTerm);
        return Ok(result);
    }

    /// <summary>
    /// Cria um novo aluno
    /// </summary>
    /// <param name="createStudentDto">Dados do aluno a ser criado</param>
    /// <returns>Aluno criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), 400)]
    public async Task<ActionResult<ApiResponse<StudentDto>>> Create([FromBody] CreateStudentDto createStudentDto)
    {
        var result = await _studentService.CreateAsync(createStudentDto);
        
        if (!result.Success)
            return BadRequest(result);
            
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Atualiza um aluno existente
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <param name="updateStudentDto">Dados atualizados do aluno</param>
    /// <returns>Aluno atualizado</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), 400)]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), 404)]
    public async Task<ActionResult<ApiResponse<StudentDto>>> Update(Guid id, [FromBody] UpdateStudentDto updateStudentDto)
    {
        var result = await _studentService.UpdateAsync(id, updateStudentDto);
        
        if (!result.Success)
        {
            if (result.Message?.Contains("não encontrado") == true)
                return NotFound(result);
                
            return BadRequest(result);
        }
            
        return Ok(result);
    }

    /// <summary>
    /// Remove um aluno (soft delete)
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>Confirmação da remoção</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<bool>), 404)]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(Guid id)
    {
        var result = await _studentService.DeleteAsync(id);
        
        if (!result.Success)
            return NotFound(result);
            
        return Ok(result);
    }

    /// <summary>
    /// Verifica se um aluno existe
    /// </summary>
    /// <param name="id">ID do aluno</param>
    /// <returns>True se o aluno existe, false caso contrário</returns>
    [HttpGet("{id:guid}/exists")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    public async Task<ActionResult<ApiResponse<bool>>> Exists(Guid id)
    {
        var result = await _studentService.ExistsAsync(id);
        return Ok(result);
    }
} 