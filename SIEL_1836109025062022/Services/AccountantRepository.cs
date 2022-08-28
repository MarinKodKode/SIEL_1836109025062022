using Dapper;
using SIEL_1836109025062022.Models.ViewModel;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IAccountantRepository
    {
        Task<IEnumerable<AccountantAuthorizationViewModel>> GetAuthorizedPayments();
        Task<IEnumerable<AccountantAuthorizationViewModel>> GetInscriptionsRequests();
        Task<IEnumerable<AccountantAuthorizationViewModel>> GetInscriptionsRequestsByProgramId(int id_program);
        Task<IEnumerable<AccountantAuthorizationViewModel>> GetUnsolvedPayments();
        Task<IEnumerable<AccountantAuthorizationViewModel>> UnauthorizedPayments();
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
        public async Task<IEnumerable<AccountantAuthorizationViewModel>> GetInscriptionsRequestsByProgramId(int id_program)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountantAuthorizationViewModel>
                (@"select * from inscriptions
                    inner join students on students.id_student = inscriptions.insc_id_student
                    inner join users on students.id_student = users.id_user
                    where insc_id_course_program = @id_program;",
                    new { id_program});
        }
        public async Task<IEnumerable<AccountantAuthorizationViewModel>> GetUnsolvedPayments()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountantAuthorizationViewModel>
                (@"select * from inscriptions
                    inner join students on students.id_student = inscriptions.insc_id_student
                    inner join users on students.id_student = users.id_user
                    where insc_status =1;");
        }
        public async Task<IEnumerable<AccountantAuthorizationViewModel>> UnauthorizedPayments()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountantAuthorizationViewModel>
                (@"select * from inscriptions
                    inner join students on students.id_student = inscriptions.insc_id_student
                    inner join users on students.id_student = users.id_user
                    where insc_status =3 or insc_status = 4;");
        }
        public async Task<IEnumerable<AccountantAuthorizationViewModel>> GetAuthorizedPayments()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountantAuthorizationViewModel>
                (@"select * from inscriptions
                    inner join students on students.id_student = inscriptions.insc_id_student
                    inner join users on students.id_student = users.id_user
                    where insc_status = 2;");
        }
    }
}
