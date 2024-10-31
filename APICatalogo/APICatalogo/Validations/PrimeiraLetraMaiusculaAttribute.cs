using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations;

public class PrimeiraLetraMaiusculaAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var primeiraLetra = value.ToString()![0].ToString();
        if (!primeiraLetra.Equals(primeiraLetra, StringComparison.CurrentCultureIgnoreCase))
            return new ValidationResult("A primeira letra do nome do produto deve ser maiuscula!");

        return ValidationResult.Success;
    }
}
