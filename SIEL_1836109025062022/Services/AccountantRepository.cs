using Dapper;
using SIEL_1836109025062022.Models.ViewModel;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IAccountantRepository
    {
        Task<IEnumerable<AccountantAuthorizationViewModel>> GetInscriptionsRequests();
    }
    public class AccountantRepository : IAccountantRepository
    {
        private readonly string connectionString;
        public AccountantRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<AccountantAuthorizationViewModel>> GetInscriptionsRequests()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountantAuthorizationViewModel>
                (@"select * from inscriptions
                    inner join students on students.id_student = inscriptions.insc_id_student
                    inner join users on students.id_student = users.id_user;");
        }
    }
}