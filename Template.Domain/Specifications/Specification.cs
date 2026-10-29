using System.Linq.Expressions;

namespace Template.Domain.Specifications;

public abstract class Specification<T>(Expression<Func<T, bool>> expression)
{
    public bool IsSatisfiedBy(T entity)
    {
        return Expression.Compile()(entity);
    }

    public Expression<Func<T, bool>> Expression { get; } = expression;
}

public class AndSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>(
        System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
                System.Linq.Expressions.Expression.AndAlso(
                    left.Expression.Body,
                    left.Expression.Parameters[0] == right.Expression.Parameters[0]
                        ? right.Expression.Body
                        : System.Linq.Expressions.Expression.Invoke(right.Expression, left.Expression.Parameters[0])),
                left.Expression.Parameters[0]));

public class OrSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>(
        System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
                System.Linq.Expressions.Expression.OrElse(
                    left.Expression.Body,
                    left.Expression.Parameters[0] == right.Expression.Parameters[0]
                        ? right.Expression.Body
                        : System.Linq.Expressions.Expression.Invoke(right.Expression, left.Expression.Parameters[0])),
                left.Expression.Parameters[0]));

public class NotSpecification<T>(Specification<T> specification) : Specification<T>(
        System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
                System.Linq.Expressions.Expression.Not(
                    specification.Expression.Body),
                specification.Expression.Parameters[0]));