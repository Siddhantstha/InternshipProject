using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validator
{
    public class ValidationActionFilterAttribute : IActionFilter, IFilterMetadata
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> list = (from e in context.ModelState.Values.SelectMany((ModelStateEntry v) => v.Errors)
                                     select e.ErrorMessage into e
                                     orderby e
                                     select e).ToList();
                JsonResult result = new JsonResult(new
                {
                    Data = 0,
                    Message = list[0],
                    Status = ResponseStatus.BadRequest
                })
                {
                    StatusCode = 400
                };
                context.Result = result;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
