using Dapper;
using Microsoft.Data.SqlClient;
using SIEL_1836109025062022.Models;


namespace SIEL_1836109025062022.Services
{
    public interface IReportsRepository
    {
        Task<IEnumerable<Student>> GetAllStudentsByCoursePrograms(int id_program);
        Task<IEnumerable<ReportProgram>> GetAllCoursePrograms();
        Task<CourseProgram> GetCourseProgramById(int id_program);
        Task<string> GetCourseProgramNameById(int id_program);
        Task<int> GetGraduatedProgram();
        Task UpdateCourseProgrma(ReportProgram reportProgram);
    }
    public class ReportsRepository : IReportsRepository
    {
        private readonly string connectionString;
        public ReportsRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        //-----------------------------MOSTRAR TODOS LOS PROGRAMAS-------------
        public async Task<IEnumerable<ReportProgram>> GetAllCoursePrograms()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<ReportProgram>(@"
                                    select * from programs;");
        }
        public async Task<CourseProgram> GetCourseProgramById(int id_program)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<CourseProgram>(@"
                         select * from programs where id_program = @id_program",
                         new { id_program });
        }
        public Task<string> GetCourseProgramNameById(int id_program)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var program_name = connection.QueryFirstOrDefaultAsync<string>(@"
                         select program_description from programs where id_program = @id_program",
                         new { id_program });
            return program_name;
        }
        public async Task<int> GetGraduatedProgram()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_program = await connection.QueryFirstOrDefaultAsync<int>(@"
                         select id_program from programs 
                            where program_name like '%gresados%'");
            return id_program;
        }

        //-------------------------------metodo mostrar estudiantes por Programa
        public async Task<IEnumerable<Student>> GetAllStudentsByCoursePrograms(int id_program)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Student>(
                                @"select u.user_name, u.user_surname, s.stdt_control_number, p.program_name 
                                from inscriptions  i
                                inner join users u
                                on u.id_user = i.insc_id_student
                                inner join students s
                                on s.id_student = i.insc_id_student
                                inner join programs p
                                on p.id_program = i.insc_id_course_program
                                where i.insc_id_course_program = @id_program
                                order by s.stdt_control_number",
                                new {id_program});
        }
        public async Task UpdateCourseProgrma(ReportProgram reportProgram)//eliminar metodo despues
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE programs
                                            set program_name = @program_name, program_description = @program_description
                                            where id_program = @id_program",
                                            reportProgram);
        }
    }
}
