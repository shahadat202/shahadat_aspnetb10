using System;

using Microsoft.AspNetCore.Identity;

namespace Blog.Infrastructure.Identity
{
    public class ApplicationUserToken
        : IdentityUserToken<Guid>
    {

    }
}
