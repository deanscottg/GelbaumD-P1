using System;
using System.ComponentModel.DataAnnotations;

namespace Barbershop.Attributes
{
    public class AppointmentHoursAttribute : ValidationAttribute
    {
        private readonly int _startHour;
        private readonly int _endHour;

        public AppointmentHoursAttribute(int startHour, int endHour)
        {
            _startHour = startHour;
            _endHour = endHour;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Hour >= _startHour && dateTime.Hour < _endHour)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(
                    $"Appointment must be scheduled between {_startHour}:00 and {_endHour}:00."
                );
            }

            // If value is null or not a DateTime, let [Required] handle that.
            return ValidationResult.Success;
        }
    }
}