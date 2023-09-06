using Core.CrossCuttingConcerns.Exceptions.Types;
using FluentValidation;
using MediatR;
using ValidationException = Core.CrossCuttingConcerns.Exceptions.Types.ValidationException;

namespace Core.Application.Pipelines.Validation
{
    //Gelen Requestin örnek, CreateCommand'in validationı var mı diye bakar
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        //Bir Command'in validator'ına ulaşacağız
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        // Her request için bir validator varsa bu handle'i çalıştır
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Validation işlemi için bir context oluşturuyoruz ve bu context'e request'i veriyoruz.
            ValidationContext<object> context = new(request);

            // Birden fazla validator'ın olduğu bir liste üzerinde dönüyoruz ve her birini request üzerinde uyguluyoruz.
            // Bu işlem sonucunda ValidationExceptionModel tipinde bir liste elde ediyoruz.
            IEnumerable<ValidationExceptionModel> errors = _validators
                .Select(validator => validator.Validate(context))
                .SelectMany(result => result.Errors) // Her validator sonucunun hatalarını birleştiriyoruz.
                .Where(failure => failure != null) // Hata olmayanları filtreliyoruz.
                .GroupBy(
                    keySelector: p => p.PropertyName, // Hataları özellik adlarına göre grupluyoruz.
                    resultSelector: (propertyName, errors) => new ValidationExceptionModel { Property = propertyName, Errors = errors.Select(e => e.ErrorMessage) }
                ).ToList();

            // Eğer hata varsa, bu hataları içeren bir ValidationException fırlatıyoruz.
            if (errors.Any())
                throw new ValidationException(errors);

            // Hata yoksa, request'i bir sonraki işleyiciye iletiyoruz ve sonucu bekliyoruz.
            TResponse response = await next();
            return response;
        }

    }
}
