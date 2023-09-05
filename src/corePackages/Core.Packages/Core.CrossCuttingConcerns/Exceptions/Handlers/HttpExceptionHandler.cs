using Core.CrossCuttingConcerns.Exceptions.Extensions;
using Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;
using Core.CrossCuttingConcerns.Exceptions.Types;
using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers
{
    public class HttpExceptionHandler : ExceptionHandler
    {
        //backing-field
        private HttpResponse? _response;
        public HttpResponse Response
        {
            get => _response ?? throw new ArgumentNullException(nameof(_response)); //defansive yaklaşım, response yok ise throw new ArgumentNullException(nameof(_response)) fırlatılsın
            set => _response = value;
        }
        //Öngördüğümüz bir hata
        protected override Task HandleException(BusinessException businessException)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            //iş kuralı oluştuğunda nasıl bir sonuç döndüereceğim? oluşan hataya göre içerik değişmektedir. Bu çözüm için ProblemDetails sınıfı kullanılır.
            string details = new BusinessProblemDetails(businessException.Message).AsJson();
            return Response.WriteAsync(details);
        }
        //Öngörmediğimiz bir hata yani Internal server 500 
        protected override Task HandleException(Exception exception)
        {
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            string details = new BusinessProblemDetails(exception.Message).AsJson();
            return Response.WriteAsync(details);
        }
    }
}
