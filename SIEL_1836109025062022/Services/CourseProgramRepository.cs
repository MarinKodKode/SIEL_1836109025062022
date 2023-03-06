using Dapper;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface ICourseProgramRepository
    {
        Task CreateCourseProgram(CourseProgram courseProgram);
        Task DeleteCourseProgramById(int id_program);
        Task<bool> ExistsCourseProgram(string program_name);
        Task<IEnumerable<CourseProgram>> GetAllCoursePrograms();
        Task<CourseProgram> GetCourseProgramById(int id_program);
        Task<string> GetCourseProgramNameById(int id_program);
        Task<int> GetGraduatedProgram();
        Task<int> GetPlacementTestId();
        Task UpdateCourseProgrma(CourseProgram courseProgram);
    }
    public class CourseProgramRepository : ICourseProgramRepository
    {
        // private readonly string connectionString;
        private readonly MySQLConfiguration connectionString;
        public CourseProgramRepository(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection MSconnection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }
        public async Task CreateCourseProgram(CourseProgram courseProgram)
        {
            var connection = MSconnection();
            var id_program = await connection.QuerySingleAsync<int>(@"
                            insert into programs (program_description, program_name) 
                            values(@program_description, @program_name);
                            SELECT LAST_INSERT_ID();",
                            courseProgram);
            courseProgram.id_program = id_program;
        }


        public async Task<bool> ExistsCourseProgram(string program_name)
        {
            var connection = MSconnection();
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 
                                            from programs
                                            where program_name = @program_name;",
                                            new { program_name });
            return exists == 1;
        }

        public async Task<IEnumerable<CourseProgram>> GetAllCoursePrograms()
        {
            var connection = MSconnection();
            return await connection.QueryAsync<CourseProgram>(@"
                                    select * from programs;");
        }

        public async Task UpdateCourseProgrma(CourseProgram courseProgram)
        {
            var connection = MSconnection();
            await connection.ExecuteAsync(@"UPDATE programs
                                            set program_name = @program_name, program_description = @program_description
                                            where id_program = @id_program",
                                            courseProgram);
        }
        public async Task<CourseProgram> GetCourseProgramById(int id_program)
        {
            var connection = MSconnection();
            return await connection.QueryFirstOrDefaultAsync<CourseProgram>(@"
                         select * from programs where id_program = @id_program",
                         new { id_program });
        }

        public async Task DeleteCourseProgramById(int id_program)
        {
            var connection = MSconnection();
            await connection.ExecuteAsync(@"
                             delete from programs where id_program = @id_program",
                             new { id_program });
        }
        public Task<string> GetCourseProgramNameById(int id_program)
        {
            var connection = MSconnection();
            var program_name = connection.QueryFirstOrDefaultAsync<string>(@"
                         select program_description from programs where id_program = @id_program",
                         new { id_program });
            return program_name;
        }
        public async Task<int> GetGraduatedProgram()
        {
            var connection = MSconnection();
            var id_program = await connection.QueryFirstOrDefaultAsync<int>(@"
                         select id_program from programs 
                            where program_name like '%gresados%'");
            return id_program;
        }

        public async Task<int> GetPlacementTestId()
        {
            var connection = MSconnection();
            var plm_Test = await connection.QueryFirstOrDefaultAsync<int>(@"
                         select * from programs where program_name like '%ment test%';");
            return plm_Test;
        }

    }
}