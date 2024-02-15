namespace Novle.Domain.Exceptions;

public class EntityDeletedException : Exception
{
    private EntityDeletedException(string entityName, int? entityId = null)
        : base($"Entity \"{entityName}\" ({entityId.ToString()}) was deleted.")
    {
    }
}