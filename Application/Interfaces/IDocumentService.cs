using PaelystSolution.Application.Dtos;
using PaelystSolution.Domain.Entities;


namespace PaelystSolution.Application.Interfaces
{
    public interface IDocumentService
    {
        DocumentsResponseModel CreateDocument(IList<IFormFile> documents);
        Task<IList<Document>> GetUserDocuments(string email);
    }
}
