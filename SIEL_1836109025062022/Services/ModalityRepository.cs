using Dapper;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
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
        //COnfiguration for connection with MySql Server
        // private readonly string connectionString;
        private readonly MySQLConfiguration connectionString;
        public ModalityRepository(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection MSconnection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }
        //CRUD OPERATIONS
        public async Task CreateModality(Modality modality)
        {
            var connection = MSconnection();
            var id_modality = await connection.QuerySingleAsync<int>
                (@"insert into modalities (modality_name,modality_description,modality_weeks_duration,modality_order,modality_level_id)
                 values(@modality_name,@modality_description, @modality_weeks_duration, @modality_order,@modality_level_id);
                 SELECT LAST_INSERT_ID()",
                 modality);
            modality.id_modality = id_modality;

        }

        public async Task<IEnumerable<Modality>> GetAllModalities()
        {
            var connection = MSconnection();
            return await connection.QueryAsync<Modality>(@"
                                    select * from modalities
                                    inner join levels 
                                    on levels.id_level = modalities.modality_level_id
                                    inner join programs
                                    on programs.id_program = levels.level_id_program;");
        }
        public async Task<IEnumerable<Modality>> GetAllModalitiesByLevel(int id_level)
        {
            var connection = MSconnection();
            return await connection.QueryAsync<Modality>(@"
                                    select * from modalities
                                    where modality_level_id = @id_level;",
                                    new { id_level });
        }
        public async Task<Modality> GetModalityById(int id_modality)
        {
            var connection = MSconnection();
            return await connection.QueryFirstOrDefaultAsync<Modality>(@"
                         select * from modalities where id_modality = @id_modality",
                         new { id_modality });
        }


        public async Task<ModalityDetailViewModel> GetModalityLevelById(int id_modality)
        {
            var connection = MSconnection();
            return await connection.QueryFirstOrDefaultAsync<ModalityDetailViewModel>(@"
                         select * from modalities
                            inner join levels 
                            on levels.id_level = modalities.modality_level_id
                            where id_modality = @id_modality",
                         new { id_modality });
        }
        public async Task DeleteModalityById(int id_modality)
        {
            var connection = MSconnection();
            await connection.ExecuteAsync(@"
                             delete FROM modalities where id_modality = @id_modality",
                             new { id_modality });
        }
        public async Task UpdateModality(Modality modality)
        {
            var connection = MSconnection();
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
            var connection = MSconnection();
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
