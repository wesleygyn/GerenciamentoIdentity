﻿using Microsoft.AspNetCore.Identity;

namespace GerenciamentoIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
    }
}
