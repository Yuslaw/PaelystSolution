using PaelystSolution.Domain.Contracts;

namespace PaelystSolution.Domain.Entities
{
    public class Document: AuditableEntity
    {
        public int? DocumentId { get; set; }
        public string Title { get; set; }
        public User? User { get; set; }  
        public Guid? UserId { get; set; }
        public string? DocumentType { get; set; }
        public long? DocumentSize { get; set; }
        public string? DocumentStream { get; set; }
        public string DocumentPath { get; set; }


    }
}
