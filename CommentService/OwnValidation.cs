using System.ComponentModel.DataAnnotations;

namespace CommentService
{
    public class ValidateYear: ValidationAttribute
    {
        private int fromYear;
        private int tillYear;

        public ValidateYear(int fromYear, int tillYear)
        {
            this.fromYear = fromYear;
            this.tillYear = tillYear;
            ErrorMessage = $"Year can be between {fromYear} and {tillYear}";
        }

        public override bool IsValid(object? value)
        {
            if (value != null && ((int)value >= fromYear && (int)value <= tillYear)) 
            { 
                return true ;
            }
            return false;
        }
    }
}
