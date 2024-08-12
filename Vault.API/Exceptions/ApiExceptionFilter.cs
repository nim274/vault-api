using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Vault.API.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ApiKeyNotFoundException:
                    HandleApiKeyNotFoundException(context);
                    break;
                case DuplicateApiKeyException:
                    HandleDuplicateApiKeyException(context);
                    break;
                default:
                    HandleInternalServerError(context);
                    break;
            }

            context.ExceptionHandled = true;
        }


        public void HandleApiKeyNotFoundException(ExceptionContext context)
        {
            var ex = context.Exception as ApiKeyNotFoundException;

            context.Result = new NotFoundObjectResult(ex.ErrorDetail)
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }

        public void HandleDuplicateApiKeyException(ExceptionContext context)
        {
            var ex = context.Exception as DuplicateApiKeyException;

            context.Result = new BadRequestObjectResult(ex.ErrorDetail)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }

        public void HandleInternalServerError(ExceptionContext context)
        {
            context.Result = new ObjectResult(new ErrorDetail(0, context.Exception?.Message!))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
