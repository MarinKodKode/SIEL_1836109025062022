using Microsoft.AspNetCore.Identity;
using SIEL_1836109025062022.Models;

namespace SIEL_1836109025062022.Services
{
    public class StudentStore : IUserStore<Student>, IUserEmailStore<Student>,
        IUserPasswordStore<Student>
    {
        private readonly IStudentsRepository studentsRepository;

        public StudentStore(IStudentsRepository studentsRepository)
        {
            this.studentsRepository = studentsRepository;
        }
        public async Task<IdentityResult> CreateAsync(Student user, CancellationToken cancellationToken)
        {
            user.id_student = await studentsRepository.CreateStudent(user);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(Student user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public async Task<Student> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return await studentsRepository.GetStudentByNormalizedEmail(normalizedEmail);
        }

        public Task<Student> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await studentsRepository.GetStudentByNormalizedEmail(normalizedUserName);

        }

        public Task<string> GetEmailAsync(Student user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.stdt_personal_email);
        }

        public Task<bool> GetEmailConfirmedAsync(Student user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(Student user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(Student user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(Student user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.stdt_hash_password);
        }

        public Task<string> GetUserIdAsync(Student user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.id_student.ToString());
        }

        public Task<string> GetUserNameAsync(Student user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.stdt_name);
        }

        public Task<bool> HasPasswordAsync(Student user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(Student user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(Student user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(Student user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.stdt_nomalized_p_email = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(Student user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(Student user, string passwordHash, CancellationToken cancellationToken)
        {
            user.stdt_hash_password = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(Student user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(Student user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
