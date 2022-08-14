using Dapper;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services


{
    public interface IModalityRepository
    {
        Task CreateModality(Modality modality);
        Task DeleteModalityById(int id_modality);
        Task<bool> ExistsModality(string modality_name);
        Task<IEnumerable<Modality>> GetAllModalities();
        Task<Level> GetModalityById(int id_modality);
        Task UpdateModality(Modality modality);
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

        public async Task UpdateModality(Modality modality)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update modalities 
                                            set modality_name = @modality_name,                                        
                                            modality_description = @modality_description,
                                            modality_weeks_duration = @modality_weeks_duration
                                            where id_modality =@id_modality", modality);
        }
        public async Task<Level> GetModalityById(int id_modality)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Level>(@"select * from modalities 
                                                                 where id_modality = @id_modality",
                                                                 new { id_modality });
        }
        public async Task DeleteModalityById(int id_modality)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
                             delete modalities where id_modality = @id_modality",
                             new { id_modality });
        }
    }
}
