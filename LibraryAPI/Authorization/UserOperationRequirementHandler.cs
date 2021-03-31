using LibraryAPI.Exceptions;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LibraryAPI.Authorization
{
    public class UserOperationRequirementHandler : AuthorizationHandler<UserOperationRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserOperationRequirement requirement, User user)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = context.User.FindFirst(x => x.Type == ClaimTypes.Role)?.Value;
            if (user.Id.ToString() == userId || role == "Admin")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}