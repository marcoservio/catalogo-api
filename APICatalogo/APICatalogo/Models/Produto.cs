using APICatalogo.Validations;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

[Table("Produtos")]
public class Produto : IValidatableObject
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    //[PrimeiraLetraMaiuscula]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(300)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [Required]
    [StringLength(300)]
    public string ImagemUrl { get; set; } = string.Empty;

    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }

    [JsonIgnore]
    public Categoria? Categoria { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrWhiteSpace(Nome))
        {
            var primeiraLetra = Nome[0].ToString();
            if (!primeiraLetra.Equals(primeiraLetra, StringComparison.CurrentCultureIgnoreCase))
            {
                yield return new ValidationResult("A primeira letra do nome do produto deve ser maiuscula!", [nameof(Nome)]);
            }
        }

        if(Estoque <= 0)
            yield return new ValidationResult("O estoque deve ser maior que zero", [nameof(Estoque)]);
    }
}
