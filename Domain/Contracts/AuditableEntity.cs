namespace PaelystSolution.Domain.Contracts
{
    public abstract class AuditableEntity : IAuditableEntity, ISoftDelete
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? DeletedOn { get; set; } = DateTime.Now;
        public int? DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        DateTime? IAuditableEntity.CreatedOn { get; set; } = DateTime.Now;
    }
}
