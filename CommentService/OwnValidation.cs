using System.ComponentModel.DataAnnotations;

namespace CommentService
{
    public class ValidateYear: ValidationAttribute
    {
        private int fromYear;
        private int NowYear;
        public ValidateYear()
        {
            fromYear = DateTime.Now.Year - 110;
            NowYear = DateTime.Now.Year;
            ErrorMessage = $"Year can be between {fromYear} and {NowYear}";
        }

        public override bool IsValid(object? value)
        {
            if (value != null && ((int)value >= fromYear && (int)value <= NowYear)) 
            { 
                return true ;
            }
            return false;
        }
    }
}
