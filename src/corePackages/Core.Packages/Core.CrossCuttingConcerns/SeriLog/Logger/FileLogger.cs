using Core.CrossCuttingConcerns.SeriLog.ConfigurationModels;
using Core.CrossCuttingConcerns.SeriLog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Core.CrossCuttingConcerns.SeriLog.Logger
{
    public class FileLogger:LoggerServiceBase
    {
        private readonly IConfiguration _configuration;

        public FileLogger(IConfiguration configuration)
        {
            _configuration = configuration;
            FileLogConfiguration logConfig = configuration.GetSection("SeriLogConfigurations:FileLogConfiguration").Get<FileLogConfiguration>() ?? throw new Exception(SeriLogMessages.NullOptionsMessage);

            string logFilePath = string.Format(format: "{0}{1}", arg0: Directory.GetCurrentDirectory() + logConfig.FolderPath, arg1: ".txt");

            //RollingInterval her gün yeni bir log dosyası oluştur diyoruz.
            Logger = new LoggerConfiguration().WriteTo.File(
                logFilePath, rollingInterval:RollingInterval.Day,
                retainedFileCountLimit:null,
                fileSizeLimitBytes:5000000,
                outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
                ).CreateLogger();
        }

    }
}
