using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace CommentService.Services
{
    public class ListErrors
    {
        public List<ModelError> GetErrors(ControllerBase controller)
        {
            var errors = new List<ModelError>();
            foreach (var modelState in controller.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errors.Add(error);
                }
            }
            return errors;
        }
    }
}
