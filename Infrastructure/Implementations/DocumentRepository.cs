using Microsoft.EntityFrameworkCore;
using PaelystSolution.Domain.Entities;
using PaelystSolution.Infrastructure.ApplicationContext;
using PaelystSolution.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace PaelystSolution.Infrastructure.Implementations
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {

        public DocumentRepository(SolutionContext context) : base(context)
        {
        }
        public async Task<Document> Get(int id)
        {
            return await _context.Documents
                .Include(a => a.User)
                 .FirstOrDefaultAsync(a => a.DocumentId == id && a.IsDeleted == false);
        }

        public async Task<IList<Document>> GetSelected(Expression<Func<Document, bool>> expression)
        {
            return await _context.Documents.Where(expression).ToListAsync();
        }

       
    }
}
