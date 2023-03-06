using Dapper;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
using SIEL_1836109025062022.Models.ViewModel;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IListRepository
    {
        Task<IEnumerable<StudentList>> GetStudentsList();
        Task<IEnumerable<StudentList>> GetStudentsListBySchedule(int id);
        Task<IEnumerable<StudentList>> GetStudentsWithActiveClasses();
    }
    public class ListRepository : IListRepository
    {
        //COnfiguration for connection with MySql Server
        // private readonly string connectionString;
        private readonly MySQLConfiguration connectionString;
        public ListRepository(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection MSconnection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }


        public async Task<IEnumerable<StudentList>> GetStudentsList()
        {
            var connection = MSconnection();
            return await connection.QueryAsync<StudentList>(@"
                            SELECT  * FROM STUDENTS 
                            inner join inscriptions on inscriptions.insc_id_student = students.id_student
                            INNER JOIN USERS on users.id_user = students.id_student
                            ;");
        }
        public async Task<IEnumerable<StudentList>> GetStudentsListBySchedule(int id)
        {
            var connection = MSconnection();
            return await connection.QueryAsync<StudentList>(@"
                            SELECT  * FROM STUDENTS 
                            inner join inscriptions on inscriptions.insc_id_student = students.id_student
                            INNER JOIN USERS on users.id_user = students.id_student
                            where insc_id_schedule = @id and insc_status = 2;",
                            new { id });
        }
        public async Task<IEnumerable<StudentList>> GetStudentsWithActiveClasses()
        {
            var connection = MSconnection();
            return await connection.QueryAsync<StudentList>(@"
                            select * from students
                            inner join users on users.id_user = students.id_student
                            inner join inscriptions on inscriptions.insc_id_student = students.id_student
                            inner join classes on classes.id_class = students.stdt_id_class
                            where classes.id_class = 1;");
        }
    }
}