namespace SIEL_1836109025062022.Data
{
    public class MySQLConfiguration
    {
        public MySQLConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; set; }
    }
}
