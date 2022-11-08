using Dapper;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.Classes;
using SIEL_1836109025062022.Models.Teacher;
using SIEL_1836109025062022.Models.Users;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface ITeachersRepository
    {
        Task<IEnumerable<User>> GetAllAdmInstitution();
        Task<IEnumerable<User>> GetAllTeachers();
        Task<int> GetTeacherActiveClasses(int id);
        Task<int> GetTeacherActiveStudents(int id);
        Task<UserDataViewModelo> GetTeacherData(int id_student);
        Task<IEnumerable<ClassCreateViewModel>> GetTeachersClasses(int id_teacher);
        Task<IEnumerable<Teacher>> GetTeachersList();
    }
    public class TeachersRepository : ITeachersRepository
    {
        // private readonly string connectionString;
        private readonly MySQLConfiguration connectionString;
        public TeachersRepository(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection MSconnection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }

        public async Task<IEnumerable<User>>GetAllTeachers()
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<User>(@"
                            select * 
                            from users
                            where users.user_id_role = 3;");
        }
        public async Task<IEnumerable<Teacher>> GetTeachersList()
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<Teacher>(@"
                            select * 
                            from users
                            where users.user_id_role = 3;");
        }
        public async Task<IEnumerable<User>> GetAllAdmInstitution()
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<User>(@"
                            select * 
                            from users
                            where users.user_id_role = 2;");
        }
        public async Task<IEnumerable<ClassCreateViewModel>> GetTeachersClasses(int id_teacher)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<ClassCreateViewModel>(@"
                            select classes.id_class, classes.group_name, schedules.schedule_description,levels.level_name, 
                            modalities.modality_name,
                            count(students.id_student) as nstudents
                            from classes
                            inner join students on students.stdt_id_class = classes.id_class
                            inner join schedules on schedules.id_schedule = group_id_schedule
                            inner join levels on levels.id_level = schedules.schedule_level
                            inner join modalities on modalities.id_modality = schedules.schedule_modality
                            where group_id_teacher = @id_teacher and classes.group_isCloded = 1
                            group by id_class, group_name,schedule_description,level_name, modality_name",
                            new { id_teacher});
        }

        public async Task<UserDataViewModelo> GetTeacherData(int id)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QuerySingleOrDefaultAsync<UserDataViewModelo>(
                @"select * from users
                    where id_user = @id;",
                new { id }
                );
        }

        public async Task<int> GetTeacherActiveClasses(int id)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var teacher_active_classes = await connection.QuerySingleAsync<int>(
                @"  SELECT COUNT(id_class)
                    FROM classes
                    WHERE group_id_teacher = @id and group_isCloded = 1; ",
                new { id });
            return teacher_active_classes;
        }

        public async Task<int> GetTeacherActiveStudents(int id)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var teacher_active_students = await connection.QuerySingleAsync<int>(
                @"  SELECT COUNT(id_student) 
                    from students
                    inner join classes on students.stdt_id_class = classes.id_class
                    inner join users on classes.group_id_teacher = users.id_user
                    where classes.group_id_teacher = @id and classes.group_isCloded = 1; ",
                new { id });
            return teacher_active_students;
        }
    }   
}
