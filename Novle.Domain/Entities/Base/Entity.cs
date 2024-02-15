using MediatR;

namespace Novle.Domain.Entities.Base;

public abstract class Entity : IEntity
{
    public int Id { get; set; }

    private readonly List<INotification> _domainEvents = new();
    
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(INotification domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
