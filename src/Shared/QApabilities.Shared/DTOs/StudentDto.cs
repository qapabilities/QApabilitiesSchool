using System.ComponentModel.DataAnnotations;

namespace QApabilities.Shared.DTOs;

public class StudentDto
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos")]
    public string Cpf { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime BirthDate { get; set; }
    
    [Required(ErrorMessage = "Telefone é obrigatório")]
    [StringLength(15, ErrorMessage = "Telefone deve ter no máximo 15 caracteres")]
    public string Phone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Endereço é obrigatório")]
    [StringLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
    public string Address { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateStudentDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos")]
    public string Cpf { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime BirthDate { get; set; }
    
    [Required(ErrorMessage = "Telefone é obrigatório")]
    [StringLength(15, ErrorMessage = "Telefone deve ter no máximo 15 caracteres")]
    public string Phone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Endereço é obrigatório")]
    [StringLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
    public string Address { get; set; } = string.Empty;
}

public class UpdateStudentDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Telefone é obrigatório")]
    [StringLength(15, ErrorMessage = "Telefone deve ter no máximo 15 caracteres")]
    public string Phone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Endereço é obrigatório")]
    [StringLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
    public string Address { get; set; } = string.Empty;
} 