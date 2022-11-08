using Dapper;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IStatusRepository
    {
        Task<IEnumerable<StatusIncription>> GetStatusInscriptionList();
    }
    public class StatusIncriptionRepostitory : IStatusRepository
    {
        // private readonly string connectionString;
        private readonly MySQLConfiguration connectionString;
        public StatusIncriptionRepostitory(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection MSconnection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }

        public async Task<IEnumerable<StatusIncription>> GetStatusInscriptionList()
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<StatusIncription>
                (@"SELECT *
                    FROM status_inscription
                    WHERE  id_status != 1
                    LIMIT 3;");
        }
    }
}
