using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails
{
    //ProblemDetails: HTTP yanıtlarında hata bilgilerini taşımak için kullanılır.
    public class BusinessProblemDetails : ProblemDetails
    {
        public BusinessProblemDetails(string detail)
        {
            Title = "Rule Violation";
            Detail = detail;
            Status = StatusCodes.Status400BadRequest;
            Type = "https://example.com/probs/business";//bu hatayı alan kişi örneğin bu sitede hataların açıklamalarını görebilir maksadında oluşturuldu
        }
    }
}
