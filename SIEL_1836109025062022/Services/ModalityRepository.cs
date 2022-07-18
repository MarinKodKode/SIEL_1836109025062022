using Dapper;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services


{
    public interface IModalityRepository
    {
        Task CreateModality(Modality modality);
        Task<bool> ExistsModality(string modality_name);
        Task<IEnumerable<Modality>> GetAllModalities();
    }
    public class ModalityRepository : IModalityRepository
    {
        private readonly string connectionString;
        public ModalityRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateModality(Modality modality)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_modality = await connection.QuerySingleAsync<int>
                (@"insert into modalities (modality_name,modality_description,modality_weeks_duration,modality_order)
                 values(@modality_name,@modality_description, @modality_weeks_duration, @modality_order);
                 SELECT SCOPE_IDENTITY();",
                 modality);
            modality.id_modality = id_modality;

        }

        public async Task<IEnumerable<Modality>> GetAllModalities()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Modality>(@"
                                    select * from modalities;");
        }

        public async Task<bool> ExistsModality(string modality_name)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 
                                            from modalities
                                            where modality_name = @modality_name;",
                                            new { modality_name });
            return exists == 1;
        }
    }
}
