using Dapper;
using Microsoft.Data.SqlClient;
using SIEL_1836109025062022.Models;

namespace SIEL_1836109025062022.Services
{

    public interface IStudentsRepository
    {
        Task<int> CreateStudent(Student student);
        Task<Student> GetStudentByNormalizedEmail(string normalizedEmail);
    }
    public class StudentsRepository : IStudentsRepository
    {
        private readonly string connectionString;

        public StudentsRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateStudent(Student student)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_student = await connection.QuerySingleAsync<int>(@"
                insert into students (stdt_name, stdt_surname, stdt_phone_1,
                stdt_phone_2,stdt_personal_email,stdt_institutional_email,
                stdt_avatar,stdt_id_institution,stdt_isLogged,stdt_id_class,
                stdt_hash_password,stdt_id_program,stdt_age,stdt_nomalized_p_email,
                stdt_normalized_i_email,stdt_control_number)
                values (@stdt_name, @stdt_surname, @stdt_phone_1,
                @stdt_phone_2,@stdt_personal_email,@stdt_institutional_email,
                @stdt_avatar,@stdt_id_institution,@stdt_isLogged,@stdt_id_class,
                @stdt_hash_password,@stdt_id_program,@stdt_age,@stdt_nomalized_p_email,
                @stdt_normalized_i_email,@stdt_control_number);
                SELECT SCOPE_IDENTITY();",
                student);

            return id_student;
        }

        public async Task<Student> GetStudentByNormalizedEmail(string stdt_nomalized_p_email)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<Student>(
                @"select * from students where stdt_nomalized_p_email= @stdt_nomalized_p_email;",
                new { stdt_nomalized_p_email }
                );
        }
    }
}
