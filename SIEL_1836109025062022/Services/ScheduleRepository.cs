using Dapper;
using SIEL_1836109025062022.Models;
using SIEL_1836109025062022.Models.ViewModel;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IScheduleRepository
    {
        Task CreateSchedule(ScheduleCreateViewModel schedule);
        Task DeleteScheduleById(int id_schedule);
        Task<bool> ExistSchedule(string schedule_name, string schedule_description);
        Task<IEnumerable<Schedule>> GetAllSchedules();
        Task<Schedule> GetSchedulebyId(int id_schedule);
        Task UpdateSchedule(Schedule schedule);
    }
    public class ScheduleRepository : IScheduleRepository
    {

        private readonly string connectionString;
        public ScheduleRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateSchedule(ScheduleCreateViewModel schedule)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_schedule = await connection.QuerySingleAsync<int>(@"insert into schedules (schedule_name, schedule_description, schedule_level)
                                values (@schedule_name,@schedule_description, @schedule_level); 
                                SELECT SCOPE_IDENTITY();",
                                schedule);
            schedule.id_schedule = id_schedule;
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Schedule>(@"
                                    select * from schedules;");
        }

        public async Task<bool> ExistSchedule(string schedule_name, string schedule_description)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 
                                            from schedule
                                            where schedule_name like @schedule_name  and schedule_description like @schedule_name",
                                            new { schedule_name, schedule_description });
            return exists == 1;
        }

        public async Task UpdateSchedule(Schedule schedule)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE schedules
                                            set schedule_name = @schedule_name, schedule_description = @schedule_description
                                            where id_schedule = @id_schedule",
                                            schedule);
        }

        public async Task<Schedule> GetSchedulebyId(int id_schedule)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Schedule>(@"
                         select * from schedules where id_schedule = @id_schedule",
                         new { id_schedule });
        }

        public async Task DeleteScheduleById(int id_schedule)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
                             delete schedules where id_schedule = @id_schedule",
                             new { id_schedule });
        }

       
    }
}