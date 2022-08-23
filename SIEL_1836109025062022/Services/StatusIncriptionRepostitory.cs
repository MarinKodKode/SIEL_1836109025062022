using Dapper;
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
        private readonly string connectionString;
        public StatusIncriptionRepostitory(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<StatusIncription>> GetStatusInscriptionList()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<StatusIncription>
                (@"select top 3 * from status_inscription where id_status != 1;");
        }
    }
}
