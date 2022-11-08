using Dapper;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.ViewModel;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{

    public interface ILevelsRepository
    {
        Task CreateLevel(Level level);
        Task DeleteLevelById(int id_level);
        Task<bool> ExistsLevel(Level level);
        Task<Level> GetLevelById(int id_level);
        Task<IEnumerable<LevelElectionViewModel>> GetLevelElection(int id_program);
        Task<int> GetLevelMinimunLevel(int id_program);
        Task<string> GetLevelPicturePath(int id_level);
        Task<IEnumerable<Level>> GetLevels();
        Task<IEnumerable<Level>> GetStudentLevelsByIdProgram(int id_program);
        Task UpdateLevel(Level level);
        Task UpdateLevelPicture(string file_path, int id_level);
    }

    public class LevelsRepository : ILevelsRepository
    {

        // private readonly string connectionString;
        private readonly MySQLConfiguration connectionString;
        public LevelsRepository(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection connection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }

        public async Task CreateLevel(Level level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            var id_level = await db.QuerySingleAsync<int>(@"
                                 insert into levels 
                                 (level_name, level_description,level_id_program, level_picture, level_order)
                                 values (@level_name, @level_description, @level_id_program , @level_picture , @level_order);
                                 SELECT LAST_INSERT_ID();",
                                 level);
            level.id_level =  id_level;    
        }
        public async Task<IEnumerable<Level>> GetLevels()
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return await db.QueryAsync<Level>(@"
                            select *
                            from levels
                            inner join programs program 
                            on program.id_program  = levels.level_id_program;");
        }
        public async Task<IEnumerable<Level>> GetStudentLevelsByIdProgram(int id_program)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return await db.QueryAsync<Level>(@"
                            select * 
                            from levels
                            inner join programs 
                            on programs.id_program = levels.level_id_program
                            where programs.id_program = @id_program;",
                            new { id_program });
        }
        public async Task<IEnumerable<LevelElectionViewModel>> GetLevelElection(int id_program)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return await db.QueryAsync<LevelElectionViewModel>(@"
                            select * 
                            from levels
                            inner join programs 
                            on programs.id_program = levels.level_id_program
                            where programs.id_program = @id_program;",
                            new { id_program });
        }
        public async Task<Level> GetLevelById(int id_level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return await db.QueryFirstOrDefaultAsync<Level>(@"select * from levels 
                                                                 where id_level = @id_level",
                                                                 new { id_level });
        }
        public async Task<bool> ExistsLevel(Level level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            var exists = await db.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 from levels
                                            where level_name like @level_name
                                            and level_id_program = @level_id_program
                                            AND level_description like @level_description;",
                                            level);
            return exists == 1;
        }
        public async Task UpdateLevel(Level level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            await db.ExecuteAsync(@"update levels 
                                            set level_name = @level_name,                                        
                                            level_description = @level_description
                                            where id_level=@id_level and level_id_program = @level_id_program", 
                                            level);
        }
        public async Task DeleteLevelById(int id_level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            await db.ExecuteAsync(@"
                             delete from levels where id_level = @id_level",
                             new { id_level });
        }
        public async Task<string> GetLevelPicturePath(int id_level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            var path = await db.QuerySingleAsync<string>(
                @"select level_picture from levels
                    where id_level = @id_level",
                new { id_level });
            return path;
        }
        public async Task UpdateLevelPicture(string file_path, int id_level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            await db.ExecuteAsync(@"update levels set level_picture = @file_path
                                            where id_level = @id_level;",
                                            new { file_path, id_level });
        }
        public async Task<int> GetLevelMinimunLevel(int id_program)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            var id_level = await db.QuerySingleAsync<int>(
                @"select MIN(id_level) from levels
                    where level_id_program = @id_program",
                new { id_program });
            return id_level;
        }

    }
}
