using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace P21_latest_template.Models.Validators
{
    public class ValidOfflineOrderTypes : ValidationAttribute
    {
        private readonly List<string> _validTypes = new List<string>(new string[] { "sales", "quote", "back_order", "standing_order" });
        public ValidOfflineOrderTypes() : base("{OrderType is not valid}")
        {
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && _validTypes.Contains(value.ToString().ToLower()))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
            //return (value !=null) && _validTypes.Contains(value.ToString()) ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
