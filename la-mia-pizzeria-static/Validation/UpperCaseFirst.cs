using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Validation
{
    public class UpperCaseFirst : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null && ((string)value)[0].ToString().ToUpper() != ((string)value)[0].ToString())
            {
                return new ValidationResult("La prima lettera dev'essere maiuscola");
            }
            return ValidationResult.Success;
        }
    }
}
