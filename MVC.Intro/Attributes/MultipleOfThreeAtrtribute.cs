using System.ComponentModel.DataAnnotations;

namespace MVC.Intro.Attributes
{
    /// <summary>
    /// Validation attribute that ensures a value is a multiple of three
    /// </summary>
    public class MultipleOfThreeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initializes a new instance of the MultipleOfThreeAttribute class
        /// </summary>
        public MultipleOfThreeAttribute()
        {
            ErrorMessage = "The value must be a multiple of three.";
        }

        /// <summary>
        /// Determines whether the specified value is valid
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>True if the value is valid, false otherwise</returns>
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return true; // Let Required attribute handle null values
            }

            if (value is int intValue)
            {
                return intValue % 3 == 0;
            }

            if (value is decimal decimalValue)
            {
                return decimalValue % 3 == 0;
            }

            if (value is double doubleValue)
            {
                return doubleValue % 3 == 0;
            }

            return false;
        }
    }
}
