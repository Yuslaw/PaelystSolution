using Microsoft.EntityFrameworkCore;
using PaelystSolution.Domain.Contracts;
using PaelystSolution.Infrastructure.ApplicationContext;
using PaelystSolution.Infrastructure.Interfaces;

namespace PaelystSolution.Infrastructure.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : AuditableEntity, new()
    {
        protected readonly SolutionContext _context;

        public BaseRepository(SolutionContext context)
        {
            _context = context;
        }

        public async Task<T> Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        
        public async Task<List<T>> AddRange(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public bool Save()
        {
            _context.SaveChanges();
            return true;
        }
    }
}
