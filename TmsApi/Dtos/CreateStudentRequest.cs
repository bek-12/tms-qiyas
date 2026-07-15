using System.ComponentModel.DataAnnotations;

namespace TmsApi.Dtos;

public record CreateStudentRequest
{
    [Required, RegularExpression(@"^qiyas-\d{4}-\d{6}$", 
        ErrorMessage = "Registration number must follow the pattern qiyas-YYYY-000000 (e.g., qiyas-2026-001268).")]
    public required string RegistrationNumber { get; init; }

    [Required, MaxLength(200)]
    public required string Name { get; init; }

    [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0.0 and 4.0.")]
    public decimal GPA { get; init; }
}