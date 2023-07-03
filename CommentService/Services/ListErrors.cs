using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace CommentService.Services
{
    public class ListErrors
    {
        public Dictionary<string, string> GetErrors(ControllerBase controller)
        {
            var errors = new Dictionary<string, string>();

            foreach (var key in controller.ModelState.Keys)
            {
                foreach (var modelState in controller.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errors[key] = error.ErrorMessage;
                    }
                }
            }
            return errors;
        }
    }
}
