using Microsoft.AspNetCore.Identity;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public IEnumerable<IdentityUser> GetUsers()
        {
            return (from user in _applicationDbContext.Users
                    select user).ToList();
        }
    }
}
