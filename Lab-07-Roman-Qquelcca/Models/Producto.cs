using System;
using System.Collections.Generic;

namespace Lab_07_Roman_Qquelcca.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string? Descripcion { get; set; }

    public int? CategoriaId { get; set; }

    public virtual Categoria? Categoria { get; set; }
}
