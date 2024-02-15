using System.Linq.Expressions;
using Novle.Domain.Entities.Base;

namespace Novle.Application.Helpers;

public interface IOrderByField
{
	dynamic GetExpression();
}

public class OrderByField<TEntity, TField> : IOrderByField where TEntity : Entity
{
	private readonly Expression<Func<TEntity, TField>> _expression;
    	
    public OrderByField(Expression<Func<TEntity, TField>> expression)
    {
    	_expression = expression;
    } 
    
    public dynamic GetExpression() => _expression;
}