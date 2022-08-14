﻿using Dapper;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.ViewModel;
using System.Data.SqlClient;


namespace SIEL_1836109025062022.Services
{

    public interface IInscriptionRepository
    {
       
        Task ApproveInscription(int id_student, int id_inscription, int insc_status);
        Task<IEnumerable<Inscription>> GetInscriptionList();
        Task<AccountantAuthorizationViewModel> GetInscriptionRequestById(int id_inscription);
        Task<CurriculumAdvance> GetLastCourseTaken(int id_student);
        Task<bool> IsStudentJoined(int id_student);
        Task MakeInscription(Inscription inscription);
    }

    public class InscriptionRepository : IInscriptionRepository
    {
        private readonly string connectionString;
        public InscriptionRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task MakeInscription(Inscription inscription)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_inscription = await connection.QuerySingleAsync<int>(@"
                                 insert into inscriptions(insc_id_student, insc_id_level,
                                    insc_id_schedule,insc_id_modality,insc_id_course_program,
                                    insc_institution, insc_file_one, insc_file_two, insc_date_time, insc_status)
                                    values (@insc_id_student,@insc_id_level,@insc_id_schedule,@insc_id_modality,
                                    @insc_id_course_program,@insc_institution,@insc_file_one,
                                    @insc_file_two, @insc_date_time,@insc_status);
                                 select SCOPE_IDENTITY();",
                                 inscription);
            inscription.id_inscription = id_inscription;
        }

        public async Task<CurriculumAdvance> GetLastCourseTaken(int id_student)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<CurriculumAdvance>(@"
                            SELECT * 
                            FROM curriculum_advance
                            WHERE id_register_curriculum_advace = 
                                (SELECT Max(id_register_curriculum_advace) 
                                 FROM curriculum_advance where crlm_id_status_level != 1) 
                            and crlm_id_student = @id_student;",
                         new { id_student });
        }

        public async Task<IEnumerable<Inscription>> GetInscriptionList()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Inscription>(@"
                    select * from inscriptions where insc_status = 1");
        }

        public async Task<bool> IsStudentJoined(int id_student)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 
                                            from inscriptions
                                            where insc_id_student = @id_student;",
                                            new { id_student });
            return exists == 1;
        }

        public async Task ApproveInscription(int insc_id_student, int id_inscription, int insc_status)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update inscriptions 
                                            set insc_status = @insc_status  
                                            where id_inscription = @id_inscription
                                            and insc_id_student = @insc_id_student",
                                            new { insc_id_student, id_inscription, insc_status });
        }

        public async Task<AccountantAuthorizationViewModel> GetInscriptionRequestById(int id_inscription)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountantAuthorizationViewModel>(@"
                         select * from inscriptions
                            inner join students on students.id_student = inscriptions.insc_id_student
                            inner join users on students.id_student = users.id_user
                            where inscriptions.id_inscription = @id_inscription;",
                         new { id_inscription });
        }
    }
}
