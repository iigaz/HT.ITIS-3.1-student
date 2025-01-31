using System;
using System.Linq.Expressions;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.UserManagement.Mapping;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

namespace Dotnet.Homeworks.Features.UserManagement.Mapping
{
    public partial class UserManagementMapper : IUserManagementMapper
    {
        public Expression<Func<User, GetUserDto>> ProjectToDto => p1 => new GetUserDto(p1.Id, p1.Name, p1.Email);
    }
}