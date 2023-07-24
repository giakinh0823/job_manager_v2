using static server.Constant.JobConstant.Method;
using System.ComponentModel.DataAnnotations;

namespace server.Validate
{
    public class AllowedHttpMethodsAttribute : ValidationAttribute
    {
        private readonly List<string> _allowedMethods = new List<string> { GET, POST, DELETE, PUT };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !_allowedMethods.Contains(value.ToString()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
