using Humanity.Domain.Models;
using Humanity.Infrastructure;
using Humanity.Repository.Interfaces;

namespace Humanity.Repository.Repositories
{
    public class UserRepository : Repository<User, string>, IUserRepository
    {
        public UserRepository(PlutoContext context) : base(context)
        {
        }
    }
}
