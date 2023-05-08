using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace la_mia_pizzeria_static.Validation
{
    public class FiveWords : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || !(value is string testo) || testo.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length < 5)
            {
                return new ValidationResult("Devi inserire una stringa di almeno 5 parole.");
            }

            return ValidationResult.Success;
        }
    }
}
