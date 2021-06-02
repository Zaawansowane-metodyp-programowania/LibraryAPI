using Microsoft.AspNetCore.Authorization;

namespace LibraryAPI.Authorization
{
    public class UserOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperation ResourceOperation { get; }


        public UserOperationRequirement(ResourceOperation resourceOperation)
        {
            ResourceOperation = resourceOperation;
        }
    }
}
