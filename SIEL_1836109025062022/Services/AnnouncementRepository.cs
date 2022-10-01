using Dapper;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IAnnouncementRepository
    {
        Task<bool> ExistsAnnouncementConcluded(int id_announcement);
        Task CreateAnnouncement(AnnouncementCreationViewModel announcement);
        Task DeleteAnnouncementById(int id_announcement);
        Task<bool> ExistsAnnouncement(AnnouncementCreationViewModel announcement);
        Task<bool> ExistsAnnouncementById(int id);
        Task<AnnouncementCreationViewModel> GetAnnouncementById(int id_announcement);
        Task<IEnumerable<Announcement>> GetAnnouncements();
        Task UpdateAnnouncement(AnnouncementCreationViewModel announcement);
    }
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly string connectionString;
        public AnnouncementRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Announcement>> GetAnnouncements()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Announcement>
                (@"select * from announcements;");
        }

        public async Task CreateAnnouncement(AnnouncementCreationViewModel announcement)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_announcement = await connection.QuerySingleAsync<int>(@"
                                 insert into announcements(announcement_name, announcement_description,
                                             announcement_picture,start_date,end_date,notes)
                                 values(@announcement_name,@announcement_description,@announcement_picture,
                                        FORMAT(@start_date, 'yyyy-MM-ddTHH:mm:ss'),@end_date,@notes);
                                 select SCOPE_IDENTITY();",
                                 announcement);
            announcement.id_announcement = id_announcement;
        }
        public async Task<AnnouncementCreationViewModel> GetAnnouncementById(int id_announcement)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AnnouncementCreationViewModel>(@"select * from announcements 
                                                                 where id_announcement = @id_announcement",
                                                                 new { id_announcement });
        }
        public async Task<bool> ExistsAnnouncement(AnnouncementCreationViewModel announcement)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 from announcements
                                            where announcement_name like @announcement_name
                                            AND announcement_description like @announcement_description
                                            AND start_date like @start_date AND end_date like @end_date;",
                                            announcement);
            return exists == 1;
        }
        public async Task<bool> ExistsAnnouncementConcluded(int id_announcement)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                                           select 1 
                                                           from announcements 
                                                           where id_announcement = @id_announcement
                                                           and end_date < GETDATE();", new { id_announcement });
            return exists == 1;
        }

        public async Task<bool> ExistsAnnouncementById(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 from announcements
                                            where id_announcement = @id;",
                                            new { id });
            return exists == 1;
        }
        public async Task UpdateAnnouncement(AnnouncementCreationViewModel announcement)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update announcements
                                            set announcement_name = @announcement_name, 
                                            announcement_description = @announcement_description,
                                            start_date = @start_date, end_date = @end_date,
                                            notes = @notes
                                            where id_announcement = 1",
                                            announcement);
        }
        public async Task DeleteAnnouncementById(int id_announcement)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
                             delete announcements where id_announcement = @id_announcement",
                             new { id_announcement });
        }

        
    }
}