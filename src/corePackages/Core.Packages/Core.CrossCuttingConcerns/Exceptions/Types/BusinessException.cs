namespace Core.CrossCuttingConcerns.Exceptions.Types
{
    public class BusinessException:Exception
    {
        //Constructor vermeden newleyip alttan ile ulaşılabilir
        public BusinessException()
        {
            
        }
        public BusinessException(string? message):base(message) { }
        public BusinessException(string? message, Exception? innerException):base(message,innerException) { }
    }
}
