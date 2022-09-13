using Dapper;
using SIEL_1836109025062022.Models.ViewModel;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IListRepository
    {
        Task<IEnumerable<StudentList>> GetStudentsList();
        Task<IEnumerable<StudentList>> GetStudentsListBySchedule(int id);
    }
    public class ListRepository : IListRepository
    {
        private readonly string connectionString;
        public ListRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<StudentList>> GetStudentsList()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<StudentList>(@"
                            SELECT  * FROM STUDENTS 
                            inner join inscriptions on inscriptions.insc_id_student = students.id_student
                            INNER JOIN USERS on users.id_user = students.id_student
                            ;");
        }
        public async Task<IEnumerable<StudentList>> GetStudentsListBySchedule(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<StudentList>(@"
                            SELECT  * FROM STUDENTS 
                            inner join inscriptions on inscriptions.insc_id_student = students.id_student
                            INNER JOIN USERS on users.id_user = students.id_student
                            inner join classes on classes.group_id_schedule = inscriptions.insc_id_schedule 
                            where insc_id_schedule = @id and insc_status = 2;",
                            new { id });
        }
    }
}
