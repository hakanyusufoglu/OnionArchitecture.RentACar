using Core.CrossCuttingConcerns.Exceptions.Types;

namespace Core.CrossCuttingConcerns.Exceptions.Handlers
{
    public abstract class ExceptionHandler
    {
        public Task HandleExceptionAsync(Exception exception) =>
            exception switch
            {
                BusinessException businessException => HandleException(businessException), //Gelen exception BusinessException türünde ise HandleException(businessException) handle et
                _ => HandleException(exception) //Switch'in defaultu
            };
        //bu sınıf dışarıya açık olmadığı için async olsa bile HandleExceptionAsync olarak isim verilmedi
        protected abstract Task HandleException(BusinessException businessException); //bunu ortamlar örneğin http için olan sınıflar implemente etsin
        protected abstract Task HandleException(Exception exception);

    }
}
