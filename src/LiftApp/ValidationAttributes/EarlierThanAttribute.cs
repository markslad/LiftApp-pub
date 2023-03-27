using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.ValidationAttributes
{
    public sealed class EarlierThanAttribute : ValidationAttribute
    {
        public EarlierThanAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success!;

            var instance = validationContext.ObjectInstance;
            if(instance is null)
            {
                throw new NullReferenceException("Validation context is null");
            }
            var otherValue = instance.GetType().GetProperty(PropertyName)!.GetValue(instance);

            if (((IComparable)value).CompareTo(otherValue) <= 0)
            {
                return ValidationResult.Success!;
            }

            return new("Neplatné časové rozpětí");
        }
    }
}
