using Microsoft.AspNetCore.Identity;

namespace WebDotNetMentoringProgram.Abstractions
{
    public interface IUserRepository
    {
        IEnumerable<IdentityUser> GetUsers();
    }
}
