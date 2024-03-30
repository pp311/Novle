namespace Novle.Domain.Exceptions;

public class EntityAlreadyExistsException : Exception
{
	public EntityAlreadyExistsException(string entityName, string? identifier = null)
	: base($"Entity \"{entityName}\" ({identifier}) already exists.")
	{
	}

	public static void ThrowIfTrue<T>(bool isEntityExists, string? identifier = null)
	{
		if (isEntityExists)
			throw new EntityAlreadyExistsException(typeof(T).Name, identifier);
	}	
}