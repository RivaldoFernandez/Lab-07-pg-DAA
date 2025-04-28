using System.ComponentModel.DataAnnotations;

namespace Lab_07_Roman_Qquelcca.DTOs;

public class CreateProductDto
{
    [Required]
    public string Nombre { get; set; }

    [Required]
    public decimal Precio { get; set; }

    public string? Descripcion { get; set; }

    public int? CategoriaId { get; set; } // Opcional
}
