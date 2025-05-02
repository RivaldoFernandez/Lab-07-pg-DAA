using System.ComponentModel.DataAnnotations;

namespace Lab_07_Roman_Qquelcca.DTOs;


public class CreateProductDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public decimal? Price { get; set; }

    public string? Description { get; set; }
}
