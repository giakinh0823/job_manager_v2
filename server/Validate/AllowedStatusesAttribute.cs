using System.ComponentModel.DataAnnotations;
using static server.Constant.JobConstant.Status;


namespace server.Validate
{
    public class AllowedStatusesAttribute : ValidationAttribute
    {
        private readonly List<string> _allowedStatuses = new List<string> { ACTIVE, INACTIVE };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !_allowedStatuses.Contains(value.ToString().ToUpper()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
