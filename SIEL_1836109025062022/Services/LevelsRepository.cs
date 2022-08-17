using Dapper;
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
            level.id_level = id_level;
        }
        public async Task<IEnumerable<Level>> GetLevels()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Level>(@"
                            select *
                            from levels
                            inner join programs program 
                            on program.id_program  = levels.level_id_program;");
        }
        public async Task<IEnumerable<Level>> GetStudentLevelsByIdProgram(int id_program)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Level>(@"
                            select * 
                            from levels
                            inner join programs 
                            on programs.id_program = levels.level_id_program
                            where programs.id_program = @id_program;",
                            new { id_program });
        }
        public async Task<IEnumerable<LevelElectionViewModel>> GetLevelElection(int id_program)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<LevelElectionViewModel>(@"
                            select * 
                            from levels
                            inner join programs 
                            on programs.id_program = levels.level_id_program
                            where programs.id_program = @id_program;",
                            new { id_program });
        }
        public async Task<Level> GetLevelById(int id_level)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Level>(@"select * from levels 
                                                                 where id_level = @id_level",
                                                                 new { id_level });
        }
        public async Task<bool> ExistsLevel(Level level)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 from levels
                                            where level_name like @level_name
                                            and level_id_program = @level_id_program
                                            AND level_description like @level_description;",
                                            level);
            return exists == 1;
        }
        public async Task UpdateLevel(Level level)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update levels 
                                            set level_name = @level_name,                                        
                                            level_description = @level_description
                                            where id_level=@id_level and level_id_program = @level_id_program",
                                            level);
        }
        public async Task DeleteLevelById(int id_level)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
                             delete levels where id_level = @id_level",
                             new { id_level });
        }
        public async Task<string> GetLevelPicturePath(int id_level)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var path = await connection.QuerySingleAsync<string>(
                @"select level_picture from levels
                    where id_level = @id_level",
                new { id_level });
            return path;
        }
        public async Task UpdateLevelPicture(string file_path, int id_level)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update levels set level_picture = @file_path
                                            where id_level = @id_level;",
                                            new { file_path, id_level });
        }
        public async Task<int> GetLevelMinimunLevel(int id_program)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_level = await connection.QuerySingleAsync<int>(
                @"select MIN(id_level) from levels
                    where level_id_program = @id_program",
                new { id_program });
            return id_level;
        }

    }
}

//using Dapper;
//using SIEL_1836109025062022.Models;
//using System.Data.SqlClient;

//namespace SIEL_1836109025062022.Services
//{

//    public interface ILevelsRepository
//    {
//        Task CreateLevel(Level level);
//        Task DeleteLevelById(int id_level);
//        Task<bool> ExistsLevel(string level_name, int level_id_program);
//        Task<Level> GetLevelById(int id_program);
//        Task<IEnumerable<Level>> GetLevels();
//        Task<IEnumerable<Level>> GetStudentLevelsByIdProgram(int id_program);
//        Task UpdateLevel(Level level);
//    }

//    public class LevelsRepository : ILevelsRepository
//    {

//        private readonly string connectionString;
//        public LevelsRepository(IConfiguration configuration)
//        {
//            connectionString = configuration.GetConnectionString("DefaultConnection");
//        }

//        public async Task CreateLevel(Level level)
//        {
//            using SqlConnection connection = new SqlConnection(connectionString);
//            var id_level = await connection.QuerySingleAsync<int>(@"
//                                 insert into levels 
//                                 (level_name, level_description,level_id_program, level_picture, level_order)
//                                 values (@level_name, @level_description, @level_id_program , @level_picture , @level_order);
//                                 select SCOPE_IDENTITY();",
//                                 level);
//            level.id_level =  id_level;    
//        }

//        public async Task<IEnumerable<Level>> GetLevels()
//        {
//            using SqlConnection connection = new SqlConnection(connectionString);
//            return await connection.QueryAsync<Level>(@"
//                            select *
//                            from levels
//                            inner join programs program 
//                            on program.id_program  = levels.level_id_program;");
//        }

//        public async Task<IEnumerable<Level>> GetStudentLevelsByIdProgram(int id_program)
//        {
//            using SqlConnection connection = new SqlConnection(connectionString);
//            return await connection.QueryAsync<Level>(@"
//                            select * 
//                            from levels
//                            inner join programs 
//                            on programs.id_program = levels.level_id_program
//                            where programs.id_program = @id_program;",
//                            new { id_program });
//        }
//        public async Task UpdateLevel(Level level)
//        {
//            using SqlConnection connection = new SqlConnection(connectionString);
//            await connection.ExecuteAsync(@"update levels 
//                                            set level_name = @level_name,                                        
//                                            level_description = @level_description,
//                                            level_picture = @level_picture,
//                                            level_order = @level_order
//                                            where id_level =@id_level", level);
//        }
//        public async Task<Level>GetLevelById(int id_level)
//        {
//            using SqlConnection connection = new SqlConnection(connectionString);
//            return await connection.QueryFirstOrDefaultAsync<Level>(@"select * from levels 
//                                                                 where id_level = @id_level",
//                                                                 new{id_level});
//        }
//        public async Task DeleteLevelById(int id_level)
//        {
//            using SqlConnection connection = new SqlConnection(connectionString);
//            await connection.ExecuteAsync(@"
//                             delete levels where id_level = @id_level",
//                             new { id_level });
//        }

//        public async Task<bool> ExistsLevel(string level_name, int level_id_program)
//        {
//            using SqlConnection connection = new SqlConnection(connectionString);
//            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
//                               select 1 
//                               from levels
//                               where level_name = @level_name 
//                               and level_id_program = @level_id_program;",
//                               new { level_name, level_id_program });
//            return exists == 1;
//        }
//    }
//}
