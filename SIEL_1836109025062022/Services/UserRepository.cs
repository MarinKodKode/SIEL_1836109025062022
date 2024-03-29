﻿using Dapper;
using MySql.Data.MySqlClient;
using SIEL_1836109025062022.Data;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface IUserRepository
    {
        Task<int> CreateUser(User user);
        Task<User> GetUserByEmail(string user_normalized_email_p);
        Task<User> GetUserById(int id_user);
        Task<string> GetUserPicturePath(int id_user);
        Task<string> GetUserProfilePicturePath(int id_user);
        Task<int> GetAsyncUserRole(int id_user);
        int GetUserRole(int id_user);
        Task<string> GetUserRoleName(int id_role);
        Task UpdateUser(User user);
        Task UpdateUserProfilePicture(string file_path, int id_user);
        Task<bool> ExistsUserEmail(string user_email);
    }
    public class UserRepository : IUserRepository
    {
        // private readonly string connectionString;
        private readonly MySQLConfiguration  connectionString;
        public UserRepository(MySQLConfiguration _connectionString)
        {
            connectionString = _connectionString;
        }

        protected MySqlConnection connection()
        {
            return new MySqlConnection(connectionString.ConnectionString);
        }

        public async Task<int> CreateUser(User user)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            var id = await db.QuerySingleAsync<int>(
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
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return await db.QuerySingleOrDefaultAsync<User>(
                @"Select * from users where  user_normalized_email_p = @user_normalized_email_p",
                new { user_normalized_email_p });
        }

        public async Task<User> GetUserById(int  id_user)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return await db.QuerySingleOrDefaultAsync<User>(
                @"Select * from users where  id_user = @id_user",
                new { id_user });
        }
        public int GetUserRole(int id_user)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return  db.QuerySingleOrDefault<int>(
                @"Select user_id_role from users where  id_user = @id_user",
                new { id_user });
        }
        public async Task<int> GetAsyncUserRole(int id_user)
        {
            // using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return await db.QuerySingleOrDefaultAsync<int>(
                @"Select user_id_role from users where  id_user = @id_user",
                new { id_user });
        }
        public async Task<string> GetUserPicturePath(int id_user)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return await db.QuerySingleOrDefaultAsync<string>(
                @"Select user_profile_picture from users where  id_user = @id_user",
                new { id_user });
        }
        public async Task<string> GetUserRoleName( int id_role)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            return await db.QuerySingleOrDefaultAsync<string>(
                @"Select role_description from roles where  id_role = @id_role",
                new { id_role });
        }

        public async Task UpdateUser(User user)
        {
            // using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            await db.ExecuteAsync(@"UPDATE users
                                            set user_personal_email = @user_personal_email, 
                                                user_institution_email= @user_institution_email,
                                                user_phone_1 = @user_phone_1,
                                                user_phone_2 = @user_phone_2
                                            where id_user = @id_user;",
                                            user);
        }

        public async Task UpdateUserProfilePicture(string file_path, int id_user)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            await db.ExecuteAsync(@"update users set user_profile_picture = @file_path
                                            where id_user = @id_user;",
                                            new { file_path, id_user});
        }

        public async Task<string> GetUserProfilePicturePath(int id_user)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            var  path = await db.QuerySingleAsync<string>(
                @"select user_profile_picture from users
                    where id_user = @id_user",
                new { id_user });
            return path;
        }

        public async Task<bool> ExistsUserEmail(string user_personal_email)
        {
            //using SqlConnection connection = new SqlConnection(connectionString);
            var db = connection();
            var exists = await db.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 
                                            from users
                                            where user_personal_email = @user_personal_email;",
                                            new { user_personal_email });
            return exists == 1;
        }
    }
}
