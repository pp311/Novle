namespace Novle.Domain.Entities.Base;

public class AuditableEntity : Entity, IAuditableEntity
{
    public int? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}