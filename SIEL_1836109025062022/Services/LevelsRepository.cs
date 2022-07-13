using Dapper;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{

    public interface ILevelsRepository
    {
        Task CreateLevel(Level level);
    }

    public class LevelsRepository : ILevelsRepository
    {

        private readonly string connectionString;
        public LevelsRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateLevel(Level level)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_level = await connection.QuerySingleAsync<int>(@"
                                 insert into levels 
                                 (level_name, level_description,level_id_program, level_picture, level_order)
                                 values (@level_name, @level_description, @level_id_program , @level_picture , @level_order);
                                 select SCOPE_IDENTITY();",
                                 level);
            level.id_level =  id_level;    
        }
    }
}
