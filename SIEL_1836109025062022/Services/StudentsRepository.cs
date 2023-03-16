using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.ViewModel;

namespace SIEL_1836109025062022.Services
{

    public interface IStudentsRepository
    {
        Task CreateCurriculumAdvanceById(CurriculumAdvance curriculum);
        Task<int> CreateStudent(Student student);
        Task CreateStudentProgramId(int id_student, int id_program);
        Task<IEnumerable<StudentSchoolarInformation>> GetAllAuthorizedStudents();
        Task<Student> GetStudentById(int id_student);
        Task<string> GetStudentControlNumber(int id_student);
        Task<int> GetStudentProgramId(int id_student);
        Task<StudentDataViewModel> GetStudentSchoolarData(int id_student);
        Task<IEnumerable<StudentSchoolarInformation>> GetStudentsInformation();
        Task<IEnumerable<StudentSchoolarInformation>> GetStudentsInformationByIdClass(int id);
        Task<Student> GetStudentUserById(int id_student);
        Task<bool> IsStudent(int id_student);
        Task<bool> IsStudentCoursing(int id_student);
        Task UpdateControlNumber(StudentDataViewModel student);
        Task UpdateStudentCoursingLevel(int student, int current_level);
        Task UpdateStudentLevel(int student, int current_level);
        Task UpdateStudentProgramId(int id_student, int id_program);
        Task<int> VerifyStudentProgramById(int id_student);
    }
    public class StudentsRepository : IStudentsRepository
    {
        // private readonly string connectionString;
        private readonly MySQLConfiguration connectionString;
        public StudentsRepository(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection MSconnection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }

        public async Task<int> CreateStudent(Student student)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var id_student = await connection.QuerySingleAsync<int>(@"
                SET IDENTITY_INSERT students ON;
                insert into students (id_student, stdt_id_class,stdt_id_program,stdt_control_number)
                values (@id_student,@stdt_id_class,@stdt_id_program,@stdt_control_number);
                SELECT SCOPE_IDENTITY();",
                student);

            return id_student;
        }

        public async Task<Student> GetStudentByNormalizedEmail(string stdt_nomalized_p_email)
        {
            //using var connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QuerySingleOrDefaultAsync<Student>(
                @"select * from students where stdt_nomalized_p_email= @stdt_nomalized_p_email;",
                new { stdt_nomalized_p_email }
                );
        }

        public async Task<Student> GetStudentById(int id_student)
        {
            //using var connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QuerySingleOrDefaultAsync<Student>(
                @"select * from students where id_student= @id_student;",
                new { id_student }
                );
        }

        public async Task<Student> GetStudentUserById(int id_student)
        {
            // using var connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QuerySingleOrDefaultAsync<Student>(
                @"select * from students
                  inner join users on  students.id_student = users.id_user
                  where id_student=@id_student and id_user = @id_student",
                new { id_student }
                );
        }

        public async Task<StudentDataViewModel> GetStudentSchoolarData(int id_student)
        {
            //using var connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QuerySingleOrDefaultAsync<StudentDataViewModel>(
                @"select * from users
                    inner join institutions on users.user_id_institution = institutions.id_institution
                    inner join students on users.id_user = students.id_student
                    where id_user = @id_student;",
                new { id_student }
                );
        }

        public async Task UpdateStudentProgramId(int id_student, int id_program)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            await connection.ExecuteAsync(@"update students set stdt_id_program = @id_program where id_student = @id_student;", new { id_student, id_program });
        }

        public async Task CreateStudentProgramId(int id_student, int stdt_id_program)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            await connection.ExecuteAsync
                (@" 
                    insert into students (id_student,stdt_id_program) values(@id_student, @stdt_id_program);",
                new { id_student, stdt_id_program });
        }

        public async Task<int> VerifyStudentProgramById(int id_student)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var id_program = await connection.QuerySingleAsync<int>(@"
                select stdt_id_program
                from students
                where id_student = @id_student; ",
                new { id_student });
            return id_program;
        }

        public async Task<bool> IsStudent(int id_student)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 
                                            from students
                                            where id_student = @id_student;",
                                            new { id_student });
            return exists == 1;
        }
        public async Task UpdateControlNumber(StudentDataViewModel student)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            await connection.ExecuteAsync(@"UPDATE students
                                            set stdt_control_number = @stdt_control_number
                                            where id_student = @id_student;",
                                            student);
        }

        public async Task CreateCurriculumAdvanceById(CurriculumAdvance curriculum)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            await connection.ExecuteAsync
                (@" insert into curriculum_advance(crlm_id_student, crlm_id_level,
                crlm_notes, crlm_certified_path, crlm_final_mark, crlm_start_date, crlm_end_date)
                values (@crlm_id_student, @crlm_id_level, 
                @crlm_notes,@crlm_certified_path,@crlm_final_mark,@crlm_start_date,@crlm_end_date)",
                curriculum);
        }

        public async Task<string> GetStudentControlNumber(int id_student)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var control_number = await connection.QuerySingleAsync<string>(@"
                select stdt_control_number
                from students
                where id_student = @id_student; ",
                new { id_student });
            return control_number;
        }
        public async Task<int> GetStudentProgramId(int id_student)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var id_program = await connection.QuerySingleAsync<int>(@"
                select stdt_id_program
                from students
                where id_student = @id_student; ",
                new { id_student });
            return id_program;
        }

        public async Task UpdateStudentLevel(int student, int current_level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            await connection.ExecuteAsync(@"update curriculum_advance 
                                            set crlm_id_status_level = 3 
                                            where crlm_id_level < @current_level 
                                            and crlm_id_student = @student",
                                            new { student, current_level });
        }

        public async Task UpdateStudentCoursingLevel(int student, int current_level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            await connection.ExecuteAsync(@"update curriculum_advance 
                                            set crlm_id_status_level = 4 
                                            where crlm_id_level = @current_level 
                                            and crlm_id_student = @student",
                                            new { student, current_level });
        }

        public async Task<bool> IsStudentCoursing(int id_student)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 
                                            from curriculum_advance 
                                            where  crlm_id_status_level = 4 
                                            and crlm_id_student = @id_student",
                                            new { id_student });
            return exists == 1;
        }
        public async Task<IEnumerable<StudentSchoolarInformation>> GetStudentsInformation()
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<StudentSchoolarInformation>(@"
                            select * from students
                            inner join users on users.id_user = students.id_student
                            inner join inscriptions on inscriptions.insc_id_student = students.id_student
                            inner join schedules on schedules.id_schedule = inscriptions.insc_id_schedule
                            inner join modalities on modalities.id_modality = schedules.schedule_modality
                            inner join levels on levels.id_level = modalities.modality_level_id
                            inner join programs on programs.id_program = levels.level_id_program
                            inner join classes on students.stdt_id_class = classes.id_class;");
        }

        public async Task<IEnumerable<StudentSchoolarInformation>> GetStudentsInformationByIdClass(int id)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<StudentSchoolarInformation>(@"
                            select * from students
                            inner join users on users.id_user = students.id_student
                            inner join inscriptions on inscriptions.insc_id_student = students.id_student
                            inner join schedules on schedules.id_schedule = inscriptions.insc_id_schedule
                            inner join modalities on modalities.id_modality = schedules.schedule_modality
                            inner join levels on levels.id_level = modalities.modality_level_id
                            inner join programs on programs.id_program = levels.level_id_program
                            inner join classes on students.stdt_id_class = classes.id_class
                            where stdt_id_class = @id;",
                            new { id });
        }
        public async Task<IEnumerable<StudentSchoolarInformation>> GetAllAuthorizedStudents()
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<StudentSchoolarInformation>(@"
                            select * from students
                            inner join users on users.id_user = students.id_student
                            inner join inscriptions on inscriptions.insc_id_student = students.id_student
                            inner join schedules on schedules.id_schedule = inscriptions.insc_id_schedule
                            inner join modalities on modalities.id_modality = schedules.schedule_modality
                            inner join levels on levels.id_level = modalities.modality_level_id
                            inner join programs on programs.id_program = levels.level_id_program
                            where inscriptions.insc_status = 2");
        }


    }
}