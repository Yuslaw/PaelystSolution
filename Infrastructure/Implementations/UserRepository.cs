using Microsoft.EntityFrameworkCore;
using PaelystSolution.Domain.Entities;
using PaelystSolution.Infrastructure.ApplicationContext;
using PaelystSolution.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace PaelystSolution.Infrastructure.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(SolutionContext context) : base(context)
        {
        }
        public async Task<User> Get(string email)
        {
            
            return await _context.Users
               .Include(a => a.Documents)
                .FirstOrDefaultAsync(a => a.UserEmail==email && a.IsDeleted == false);
        }

        public async Task<User> Get(Expression<Func<User, bool>> expression)
        {
            return await _context.Users.Include(x => x.Documents)
                .FirstOrDefaultAsync(expression);
        }
    }
}
