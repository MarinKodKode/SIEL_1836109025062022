using Dapper;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.ViewModel;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IScheduleRepository
    {
        Task CreateSchedule(Schedule schedule);
        Task DeleteScheduleById(int id_schedule);
        Task<bool> ExistSchedule(string schedule_name, string schedule_description);
        Task<bool> ExistsSchedule(Schedule schedule);
        Task<IEnumerable<Schedule>> GetAllSchedules();
        Task<IEnumerable<Schedule>> GetAllSchedulesByLevel(int id_level);
        Task<IEnumerable<Schedule>> GetAllSchedulesByModality(int id);
        Task<Schedule> GetSchedulebyId(int id_schedule);
        Task UpdateSchedule(Schedule schedule);
    }
    public class ScheduleRepository : IScheduleRepository
    {

        // private readonly string connectionString;
        private readonly MySQLConfiguration connectionString;
        public ScheduleRepository(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection MSconnection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }

        public async Task CreateSchedule(Schedule schedule)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var id_schedule = await connection.QuerySingleAsync<int>(@"insert into schedules (schedule_name, schedule_description, schedule_level,schedule_modality)
                                values (@schedule_name,@schedule_description, @schedule_level,@schedule_modality); 
                                SELECT LAST_INSERT_ID();",
                                schedule);
            schedule.id_schedule = id_schedule;
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules()
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<Schedule>(@"
                                    select * from schedules;");
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedulesByLevel(int id_level)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<Schedule>(@"
                                    select * from schedules
                                    inner join modalities on schedules.schedule_modality = modalities.id_modality
                                    where schedule_level = @id_level;",
                                    new { id_level });
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedulesByModality(int id)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryAsync<Schedule>(@"
                                    select * from schedules
                                    where schedule_modality = @id;",
                                    new { id });
        }
        public async Task<bool> ExistSchedule(string schedule_name, string schedule_description)
        {
            // using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 
                                            from schedule
                                            where schedule_name like @schedule_name  and schedule_description like @schedule_name",
                                            new { schedule_name, schedule_description });
            return exists == 1;
        }

        public async Task UpdateSchedule(Schedule schedule)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            await connection.ExecuteAsync(@"UPDATE schedules
                                            set schedule_name = @schedule_name, schedule_description = @schedule_description
                                            where id_schedule = @id_schedule",
                                            schedule);
        }

        public async Task<Schedule> GetSchedulebyId(int id_schedule)
        {
            // using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            return await connection.QueryFirstOrDefaultAsync<Schedule>(@"
                         select * from schedules
                            inner join levels on levels.id_level = schedules.schedule_level
                            inner join modalities on modalities.id_modality = schedules.schedule_modality
                            inner join programs on levels.level_id_program = programs.id_program
                            where id_schedule = @id_schedule",
                         new { id_schedule });
        }

        public async Task DeleteScheduleById(int id_schedule)
        {
            // using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            await connection.ExecuteAsync(@"
                             delete schedules where id_schedule = @id_schedule",
                             new { id_schedule });
        }

        //Validations

        public async Task<bool> ExistsSchedule(Schedule schedule)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var connection = MSconnection();
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 from schedules
                                            where schedule_name like @schedule_name
                                            and schedule_level = @schedule_level
                                            AND schedule_description like @schedule_description
                                            AND schedule_modality like @schedule_modality;",
                                            schedule);
            return exists == 1;
        }


    }
}