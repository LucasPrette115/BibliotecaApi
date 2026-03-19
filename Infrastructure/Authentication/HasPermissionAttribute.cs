using Microsoft.AspNetCore.Authorization;

namespace BibliotecaApi.Infrastructure.Authentication
{
    public sealed class HasPermissionAttribute(Permission permission) : AuthorizeAttribute(policy: permission.ToString())
    {
    }
}
