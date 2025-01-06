using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.UpdateUser;

public class UpdateUserValidator: AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(update => update.User.Email).EmailAddress();
        RuleFor(update => update.User.Name).Length(1, 50);
    }
}