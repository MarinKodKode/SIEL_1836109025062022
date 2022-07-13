﻿using Dapper;
using SIEL_1836109025062022.Models;
using System.Data.SqlClient;

namespace SIEL_1836109025062022.Services
{
    public interface ICourseProgramRepository
    {
        Task CreateCourseProgram(CourseProgram courseProgram);
        Task DeleteCourseProgramById(int id_program);
        Task<bool> ExistsCourseProgram(string program_name);
        Task<IEnumerable<CourseProgram>> GetAllCoursePrograms();
        Task<CourseProgram> GetCourseProgramById(int id_program);
        Task UpdateCourseProgrma(CourseProgram courseProgram);
    }
    public class CourseProgramRepository : ICourseProgramRepository
    {
        private readonly string connectionString;
        public CourseProgramRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public  async Task CreateCourseProgram(CourseProgram courseProgram)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var id_program = await connection.QuerySingleAsync<int>(@"
                            insert into programs (program_description, program_name) 
                            values(@program_description, @program_name);
                            SELECT SCOPE_IDENTITY();",
                            courseProgram);
            courseProgram.id_program = id_program; 
        }


        public async Task<bool> ExistsCourseProgram(string program_name)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(@"
                                            select 1 
                                            from programs
                                            where program_name = @program_name;",
                                            new { program_name});
            return exists == 1;
        }

        public async Task<IEnumerable<CourseProgram>> GetAllCoursePrograms()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<CourseProgram>(@"
                                    select * from programs;") ;
        }

        public async Task UpdateCourseProgrma(CourseProgram courseProgram)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE programs
                                            set program_name = @program_name, program_description = @program_description
                                            where id_program = @id_program", 
                                            courseProgram);
        }
        public async Task<CourseProgram> GetCourseProgramById(int id_program)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<CourseProgram>(@"
                         select * from programs where id_program = @id_program",
                         new { id_program});
        }

        public async Task DeleteCourseProgramById(int id_program)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
                             delete programs where id_program = @id_program",
                             new { id_program});
        }
    }
}
