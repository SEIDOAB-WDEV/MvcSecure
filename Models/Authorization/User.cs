using Microsoft.AspNetCore.Identity;

namespace Models.Authorization;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}


