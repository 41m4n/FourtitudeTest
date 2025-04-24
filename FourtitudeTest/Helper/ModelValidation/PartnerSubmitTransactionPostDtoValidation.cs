using System.ComponentModel.DataAnnotations;

namespace FourtitudeTest.Helper.ModelValidation
{
    public class PartnerSubmitTransactionPostDtoValidation
    {
        public class ValidTimestampAttribute : ValidationAttribute
        {
            private readonly int _minMaxMinutes;

            public ValidTimestampAttribute(int minMaxMinutes = 5)
            {
                _minMaxMinutes = minMaxMinutes;
                ErrorMessage = $"Expired.";
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is not string timestampStr)
                    return new ValidationResult("Invalid timestamp format");

                if (!DateTime.TryParse(timestampStr, out var requestTime))
                    return new ValidationResult("Timestamp could not be parsed");

                var serverTimeUtc = DateTime.UtcNow;
                var lowerBound = serverTimeUtc.AddMinutes(-_minMaxMinutes);
                var upperBound = serverTimeUtc.AddMinutes(_minMaxMinutes);

                if (requestTime < lowerBound || requestTime > upperBound)
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
        }
    }
}
