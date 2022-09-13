using Dapper;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface ITeachersRepository
    {
        Task<IEnumerable<User>> GetAllAdmInstitution();
        Task<IEnumerable<User>> GetAllTeachers();
    }
    public class TeachersRepository : ITeachersRepository
    {
        private readonly string connectionString;

        public TeachersRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<User>>GetAllTeachers()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<User>(@"
                            select * 
                            from users
                            where users.user_id_role = 3;");
        }
        public async Task<IEnumerable<User>> GetAllAdmInstitution()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<User>(@"
                            select * 
                            from users
                            where users.user_id_role = 2;");
        }
    }
}
