

using PaelystSolution.Domain.Entities;
using System.Linq.Expressions;

namespace PaelystSolution.Infrastructure.Interfaces
{
    public interface IDocumentRepository: IBaseRepository<Document>
    {
        Task<Document> Get(int id);
        Task<IList<Document>> GetSelected(Expression<Func<Document, bool>> expression);
    }
}
