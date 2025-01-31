using System.Linq.Expressions;
using System.Security.Authentication;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation.Results;

namespace Dotnet.Homeworks.Infrastructure.Validation.Behaviors;

internal static class BehavioursHelper
{
    public static TResponse ToArbitraryResult<TResponse>(PermissionResult permissionResult) where TResponse: Result
    {
        if (typeof(PermissionResult).IsAssignableTo(typeof(TResponse)))
            return (permissionResult as TResponse)!;
        return ToArbitraryResult<TResponse>(permissionResult as Result);
    }
    public static TResponse ToArbitraryResult<TResponse>(ValidationResult validationResult) where TResponse: Result
    {
        return ToArbitraryResult<TResponse>(new Result(validationResult.IsValid,
            validationResult.Errors.Select(failure => failure.ErrorMessage).FirstOrDefault()));
    }
    public static TResponse ToArbitraryResult<TResponse>(Result result) where TResponse: Result
    {
        if (typeof(Result).IsAssignableTo(typeof(TResponse)))
            return (new Result(result.IsSuccess, result.Error) as TResponse)!;
        if (typeof(TResponse).IsGenericType &&
            typeof(Result<>).IsAssignableTo(typeof(TResponse).GetGenericTypeDefinition()))
        {
            var arg = typeof(TResponse).GenericTypeArguments[0];
            var func = Expression.Lambda<Func<TResponse>>(Expression.New(
                typeof(TResponse).GetConstructor(new[] { arg, typeof(bool), typeof(string) })!, Expression.Default(arg),
                Expression.Constant(result.IsSuccess), Expression.Constant(result.Error))).Compile();
            return func();
        }
        throw new InvalidCredentialException(result.Error);
    }
}