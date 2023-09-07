using Serilog;

namespace Core.CrossCuttingConcerns.SeriLog
{
    //Loglamayı yapan sınıf ancak bu file log, mongodb loglama yapabilceğim için bu sınıfı abstract class olarak tanımladım. İlgili sınıf override ederek bu sınıfı kullanabilecektir.
    public abstract class LoggerServiceBase
    {
        //Sadece inherit eden yer görsün diye protected verdim
        protected ILogger Logger { get; set; }

        //Verdiğimiz loggerin implemantasyonu sağlanıyor.
        protected LoggerServiceBase()
        {
            Logger = null;
        }
        protected LoggerServiceBase(ILogger logger)
        {
            Logger = logger;
        }
        //Verbose detaylı log anlamında
        public void Verbose(string message) => Logger.Verbose(message);
        public void Fatal(string message) => Logger.Fatal(message);
        public void Error(string message) => Logger.Error(message);
        public void Info(string message) => Logger.Information(message);
        public void Warn(string message) => Logger.Warning(message);
    }
}
