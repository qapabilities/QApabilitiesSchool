using System.ComponentModel.DataAnnotations;

namespace QApabilities.Shared.DTOs;

public class CourseDto
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Nome do curso é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, 1000, ErrorMessage = "Duração deve estar entre 1 e 1000 horas")]
    public int DurationHours { get; set; }
    
    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Capacidade máxima é obrigatória")]
    [Range(1, 1000, ErrorMessage = "Capacidade deve estar entre 1 e 1000 alunos")]
    public int MaxCapacity { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateCourseDto
{
    [Required(ErrorMessage = "Nome do curso é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, 1000, ErrorMessage = "Duração deve estar entre 1 e 1000 horas")]
    public int DurationHours { get; set; }
    
    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Capacidade máxima é obrigatória")]
    [Range(1, 1000, ErrorMessage = "Capacidade deve estar entre 1 e 1000 alunos")]
    public int MaxCapacity { get; set; }
}

public class UpdateCourseDto
{
    [Required(ErrorMessage = "Nome do curso é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500, ErrorMessage = "Descrição deve ter no máximo 500 caracteres")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Duração é obrigatória")]
    [Range(1, 1000, ErrorMessage = "Duração deve estar entre 1 e 1000 horas")]
    public int DurationHours { get; set; }
    
    [Required(ErrorMessage = "Preço é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Price { get; set; }
    
    [Required(ErrorMessage = "Capacidade máxima é obrigatória")]
    [Range(1, 1000, ErrorMessage = "Capacidade deve estar entre 1 e 1000 alunos")]
    public int MaxCapacity { get; set; }
} 