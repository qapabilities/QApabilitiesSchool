using System.ComponentModel.DataAnnotations;

namespace QApabilities.Shared.DTOs;

public class EnrollmentDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    
    [Required(ErrorMessage = "Data de matrícula é obrigatória")]
    public DateTime EnrollmentDate { get; set; }
    
    [Required(ErrorMessage = "Status é obrigatório")]
    public EnrollmentStatus Status { get; set; }
    
    public decimal? Grade { get; set; }
    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Propriedades de navegação
    public StudentDto? Student { get; set; }
    public CourseDto? Course { get; set; }
}

public class CreateEnrollmentDto
{
    [Required(ErrorMessage = "ID do aluno é obrigatório")]
    public Guid StudentId { get; set; }
    
    [Required(ErrorMessage = "ID do curso é obrigatório")]
    public Guid CourseId { get; set; }
    
    public string? Notes { get; set; }
}

public class UpdateEnrollmentDto
{
    [Required(ErrorMessage = "Status é obrigatório")]
    public EnrollmentStatus Status { get; set; }
    
    [Range(0, 10, ErrorMessage = "Nota deve estar entre 0 e 10")]
    public decimal? Grade { get; set; }
    
    public string? Notes { get; set; }
}

public enum EnrollmentStatus
{
    Pending = 0,
    Active = 1,
    Completed = 2,
    Cancelled = 3,
    Suspended = 4
} 