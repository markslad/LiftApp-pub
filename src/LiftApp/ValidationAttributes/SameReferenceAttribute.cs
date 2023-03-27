using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.ValidationAttributes
{
    public sealed class SameReferenceAttribute : ValidationAttribute
    {
        public SameReferenceAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            if (instance is null)
            {
                throw new NullReferenceException("Validation context is null");
            }
            var otherValue = instance.GetType().GetProperty(PropertyName)!.GetValue(instance);

            if ((value is null && otherValue is null) ||
                (value is not null && otherValue is not null))
            {
                return ValidationResult.Success!;
            }

            return new("Chybějící část datového rozpětí");
        }
    }
}
