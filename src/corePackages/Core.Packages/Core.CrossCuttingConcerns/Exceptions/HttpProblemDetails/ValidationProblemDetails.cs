using Core.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails
{
    public class ValidationProblemDetails:ProblemDetails
    {
        //init sayesinde Errors'a sadece tek bir değer atanabilir ve bu değer sonrasında değiştirilmesini engellenmesini sağlar.
        public IEnumerable<ValidationExceptionModel> Errors { get; init; }
        public ValidationProblemDetails(IEnumerable<ValidationExceptionModel> errors)
        {
            Title = "Validation error(s)";
            Detail = "One or more validation errors occured";
            Errors = errors;
            Status = StatusCodes.Status400BadRequest;
            Type = "https://example.com/probs/validation";
        }
    }
}
