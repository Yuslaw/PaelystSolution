using Microsoft.EntityFrameworkCore;
using PaelystSolution.Domain.Entities;
using System.Linq.Expressions;

namespace PaelystSolution.Infrastructure.Interfaces
{
    public interface IUserRepository: IBaseRepository<User>
    {
        Task<User> Get(string email);
        Task<User> Get(Expression<Func<User, bool>> expression);
       
    }
}
