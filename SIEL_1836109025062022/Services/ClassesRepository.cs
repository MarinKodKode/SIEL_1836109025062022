using Dapper;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
using SIEL_1836109025062022.Models.Classes;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IClassesRepository
    {
        Task AssignClassToStudents(ClassCreateViewModel myNewClass);
        Task<int> CreateClass(ClassCreateViewModel myNewClass);
        Task DeleteClass(int class_to_delete);
        Task<ClassCreateViewModel> GetClassById(int id_class);
        Task<IEnumerable<ClassCreateViewModel>> GetClasses();
        Task ResetClassToStudents(int class_to_reset);
        Task UpdateClass(ClassCreateViewModel class_to_update);
    }
    public class ClassesRepository : IClassesRepository
    {
        // private readonly string connectionString;
        private readonly MySQLConfiguration connectionString;
        public ClassesRepository(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection MSconnection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }


        public async Task<int> CreateClass(ClassCreateViewModel myNewClass)
        {
            var connection = MSconnection();
            var id_class = await connection.QuerySingleAsync<int>(@"
                                 insert into classes
                                        (	classes.group_name,
	                                        classes.group_id_responsable,
	                                        classes.group_id_teacher,
                                            classes.group_id_schedule)
                                        values 
                                        (
	                                        @group_name,
	                                        @group_id_responsable,
                                            @group_id_teacher,
                                            @group_id_schedule);
                                        SELECT LAST_INSERT_ID();",
                                 myNewClass);
            myNewClass.id_class = id_class;
            return id_class;
        }
        public async Task<IEnumerable<ClassCreateViewModel>> GetClasses()
        {
            var connection = MSconnection();
            return await connection.QueryAsync<ClassCreateViewModel>(@"
                            select *
                            from classes
                            inner join users on users.id_user = classes.group_id_teacher
                            inner join schedules on schedules.id_schedule = classes.group_id_schedule
                            inner join modalities on modalities.id_modality = schedules.schedule_modality
                            inner join levels on levels.id_level = modalities.modality_level_id
                            inner join programs on programs.id_program = levels.level_id_program;");
        }

        public async Task AssignClassToStudents(ClassCreateViewModel myNewClass)
        {
            var connection = MSconnection();
            await connection.ExecuteAsync(@"update students 
                                            inner join inscriptions 
                                            on inscriptions.insc_id_student = students.id_student
                                            set stdt_id_class = @assignedClass 
                                            where insc_status = 2 and insc_id_schedule = @group_id_schedule 
                                            and (stdt_id_class = 4 OR stdt_id_class IS NULL)
                                            ORDER BY students.id_student
                                            LIMIT @studentsAssigned;",
                                            myNewClass);
        }
        public async Task<ClassCreateViewModel> GetClassById(int id_class)
        {
            var connection = MSconnection();
            return await connection.QueryFirstOrDefaultAsync<ClassCreateViewModel>
                (@"select *
                    from classes
                    inner join users on users.id_user = classes.group_id_teacher
                    inner join schedules on schedules.id_schedule = classes.group_id_schedule
                    inner join modalities on modalities.id_modality = schedules.schedule_modality
                    inner join levels on levels.id_level = modalities.modality_level_id
                    inner join programs on programs.id_program = levels.level_id_program
                    where id_class = @id_class;",
                    new { id_class });
        }
        public async Task UpdateClass(ClassCreateViewModel class_to_update)
        {
            var connection = MSconnection();
            await connection.ExecuteAsync(@"update classes 
                                            set group_name = @group_name,                                        
                                            group_id_responsable = @group_id_responsable,
                                            group_id_teacher = @group_id_teacher
                                            where id_class=@id_class",
                                            class_to_update);
        }

        public async Task ResetClassToStudents(int class_to_reset)
        {
            var connection = MSconnection();
            await connection.ExecuteAsync(@"update students 
                                            set stdt_id_class = null
                                            where stdt_id_class=@class_to_reset",
                                            new { class_to_reset });
        }

        public async Task DeleteClass(int class_to_delete)
        {
            var connection = MSconnection();
            await connection.ExecuteAsync(@"delete from classes 
                                            where id_class=@class_to_delete",
                                            new { class_to_delete });
        }
    }
}
