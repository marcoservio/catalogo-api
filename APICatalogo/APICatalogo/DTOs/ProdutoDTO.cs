using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs;

public class ProdutoDTO
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(300)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string ImagemUrl { get; set; } = string.Empty;

    public int CategoriaId { get; set; }
}
