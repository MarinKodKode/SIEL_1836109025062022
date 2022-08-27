using Dapper;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.ViewModel;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services


{
    public interface IModalityRepository
    {
        Task CreateModality(Modality modality);
        Task DeleteModalityById(int id_modality);
        Task<bool> ExistsModality(Modality modality);
        Task<IEnumerable<Modality>> GetAllModalities();
        Task<IEnumerable<Modality>> GetAllModalitiesByLevel(int id_level);
        Task<Modality> GetModalityById(int id_modality);
        Task<ModalityDetailViewModel> GetModalityLevelById(int id_modality);
        Task UpdateModality(Modality modality);
    }
    public class ModalityRepository : IModalityRepository
    {
        private readonly string connectionString;
        public ModalityRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //CRUD OPERATIONS
        public async Task CreateModality(Modality modality)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_modality = await connection.QuerySingleAsync<int>
                (@"insert into modalities (modality_name,modality_description,modality_weeks_duration,modality_order,modality_level_id)
                 values(@modality_name,@modality_description, @modality_weeks_duration, @modality_order,@modality_level_id);
                 SELECT SCOPE_IDENTITY();",
                 modality);
            modality.id_modality = id_modality;

        }

        public async Task<IEnumerable<Modality>> GetAllModalities()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Modality>(@"
                                    select * from modalities
                                    inner join levels 
                                    on levels.id_level = modalities.modality_level_id
                                    inner join programs
                                    on programs.id_program = levels.level_id_program;");
        }
        public async Task<IEnumerable<Modality>> GetAllModalitiesByLevel(int id_level)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Modality>(@"
                                    select * from modalities
                                    where modality_level_id = @id_level;",
                                    new { id_level });
        }
        public async Task<Modality> GetModalityById(int id_modality)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Modality>(@"
                         select * from modalities where id_modality = @id_modality",
                         new { id_modality });
        }


        public async Task<ModalityDetailViewModel> GetModalityLevelById(int id_modality)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<ModalityDetailViewModel>(@"
                         select * from modalities
                            inner join levels 
                            on levels.id_level = modalities.modality_level_id
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
        public async Task UpdateModality(Modality modality)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update modalities
                                            set modality_name = @modality_name,
                                            modality_description = @modality_description,
                                            modality_weeks_duration = @modality_weeks_duration
                                            where id_modality = @id_modality;",
                                            modality);
        }
        //VALIDATIONS
        public async Task<bool> ExistsModality(Modality modality)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 from modalities
                                            where modality_name like @modality_name
                                            and modality_level_id = @modality_level_id
                                            AND modality_description like @modality_description;",
                                            modality);
            return exists == 1;
        }
    }
}