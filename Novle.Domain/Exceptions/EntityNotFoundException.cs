namespace Novle.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, int? entityId = null)
        : base($"Entity \"{entityName}\" ({entityId.ToString()}) was not found.")
    {
    } 
    
    /// <summary>
    /// Throws an EntityNotFoundException if the entity is null
    /// </summary>
    public static void ThrowIfNull<T>(T entity, int entityId)
    {
        if (entity == null)
            throw new EntityNotFoundException(typeof(T).Name, entityId);
    }
    
    public static void ThrowIfFalse<T>(bool isEntityExists, int? entityId = null)
    {
        if (!isEntityExists)
            throw new EntityNotFoundException(typeof(T).Name, entityId);
    }
}