namespace Core.CrossCuttingConcerns.SeriLog.ConfigurationModels
{
    public class MsSqlConfiguration
    {
        //Mssql'e yazabilmek için connectionstringe ihtiyacımız var
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        //Log tablosu yoksa oluşturulsun mu?
        public bool AutoCreateSqlTable { get; set; }

        public MsSqlConfiguration()
        {
            ConnectionString = string.Empty;
            TableName = string.Empty;
        }
        public MsSqlConfiguration(string connectionString, string tableName, bool autoCreateSqlTaable)
        {
            ConnectionString = connectionString;
            TableName = tableName;
            AutoCreateSqlTable = autoCreateSqlTaable;
        }
    }
}
