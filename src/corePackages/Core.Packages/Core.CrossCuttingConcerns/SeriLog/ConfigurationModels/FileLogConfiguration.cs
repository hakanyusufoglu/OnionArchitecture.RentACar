namespace Core.CrossCuttingConcerns.SeriLog.ConfigurationModels
{
    public class FileLogConfiguration
    {
        //Log yapılacak dosyanın yolu
        public string FolderPath { get; set; }
        public FileLogConfiguration()
        {
            FolderPath = string.Empty;
        }
        public FileLogConfiguration(string folderPath)
        {
            FolderPath = folderPath;
        }
    }

}
