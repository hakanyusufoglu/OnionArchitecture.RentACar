using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.SeriLog;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Core.Application.Pipelines.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ILoggableRequest
    {
        //İlgili httpcontexteki bilgileri, kullanıcı bilgilerine erişmemizi sağlar
        private readonly IHttpContextAccessor _httpContextAccessor;
        //Bunun sayesinde istersek file log istersek de mongodbye loglama yapabiliriz
        private readonly LoggerServiceBase _loggerServiceBase;

        public LoggingBehavior(IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerServiceBase)
        {
            _httpContextAccessor = httpContextAccessor;
            _loggerServiceBase = loggerServiceBase;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<LogParameter> logParameters = new List<LogParameter>
            {
                new LogParameter{Type=request.GetType().Name, Value=request}
            };
            LogDetail logDetail = new LogDetail
            {
                MethodName = next.Method.Name,
                Parameters = logParameters,
                User = _httpContextAccessor.HttpContext.User.Identity?.Name ?? "?"
            };
            _loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));
            return await next();
        }
    }
}
