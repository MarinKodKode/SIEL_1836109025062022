using Dapper;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IUserRepository
    {
        Task<int> CreateUser(User user);
        Task<User> GetUserByEmail(string user_normalized_email_p);
        Task UpdateUser(User user);
        Task UpdateUserProfilePicture(string file_path, int id_user);
    }
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;
        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CreateUser(User user)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"                   
                                     insert into users (
                                     user_name, user_surname, user_personal_email,
                                     user_id_institution, user_id_role,
                                     user_hash_password, user_normalized_email_p, user_phone_1)
                                     values(@user_name, @user_surname, @user_personal_email, 
                                     @user_id_institution,@user_id_role,@user_hash_password,
                                     @user_normalized_email_p,@user_phone_1);
                                        SELECT SCOPE_IDENTITY();", user) ;
            return id;
        }

        public async Task<User> GetUserByEmail(string user_normalized_email_p)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QuerySingleOrDefaultAsync<User>(
                @"Select * from users where  user_normalized_email_p = @user_normalized_email_p",
                new { user_normalized_email_p });
        }

        public async Task UpdateUser(User user)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE users
                                            set user_personal_email = @user_personal_email, 
                                                user_institution_email= @user_institution_email,
                                                user_phone_1 = @user_phone_1,
                                                user_phone_2 = @user_phone_2
                                            where id_user = @id_user;",
                                            user);
        }

        public async Task UpdateUserProfilePicture(string file_path, int id_user)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"update users set user_profile_picture = @file_path
                                            where id_user = @id_user;",
                                            new { file_path, id_user});
        }

    }
}
