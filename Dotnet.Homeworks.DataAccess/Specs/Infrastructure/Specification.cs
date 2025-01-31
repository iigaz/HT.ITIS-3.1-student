using System.Linq.Expressions;

namespace Dotnet.Homeworks.DataAccess.Specs.Infrastructure;

public class Specification<T> : IQueryableFilter<T> where T : class
{
    public Expression<Func<T, bool>> Expression { get; }

    public Specification(Expression<Func<T, bool>> expression)
    {
        Expression = expression;
        if (expression == null) throw new ArgumentNullException(nameof(expression));
    }

    public static implicit operator Expression<Func<T, bool>>(Specification<T> spec)
    {
        return spec.Expression;
    }

    public static bool operator false(Specification<T> spec)
    {
        return false;
    }

    public static bool operator true(Specification<T> spec)
    {
        return false;
    }

    public static Specification<T> operator &(Specification<T> spec1, Specification<T> spec2)
    {
        return new Specification<T>(spec1.Expression.And(spec2.Expression));
    }

    public static Specification<T> operator |(Specification<T> spec1, Specification<T> spec2)
    {
        return new Specification<T>(spec1.Expression.Or(spec2.Expression));
    }

    public static Specification<T> operator !(Specification<T> spec)
    {
        return new Specification<T>(spec.Expression.Not());
    }

    public IQueryable<T> Apply(IQueryable<T> query)
    {
        return query.Where(Expression);
    }

    public bool IsSatisfiedBy(T obj)
    {
        return Expression.Compile()(obj);
    }
}