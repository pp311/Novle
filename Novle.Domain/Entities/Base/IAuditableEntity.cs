namespace Novle.Domain.Entities.Base;

public interface IAuditableEntity : IEntity
{
    int? CreatedBy { get; set; }  
    DateTime? CreatedOn { get; set; }
    int? UpdatedBy { get; set; }
    DateTime? UpdatedOn {get;set;}
}