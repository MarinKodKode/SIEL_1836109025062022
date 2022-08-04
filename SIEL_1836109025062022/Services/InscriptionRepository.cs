using Dapper;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;


namespace SIEL_1836109025062022.Services
{

    public interface IInscriptionRepository
    {
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
                                    insc_institution, insc_file_one, insc_file_two, insc_date_time)
                                    values (@insc_id_student,@insc_id_level,@insc_id_schedule,@insc_id_modality,
                                    @insc_id_course_program,@insc_institution,@insc_file_one,
                                    @insc_file_two, @insc_date_time);
                                 select SCOPE_IDENTITY();",
                                 inscription);
            inscription.id_inscription = id_inscription;
        }

    }
}
