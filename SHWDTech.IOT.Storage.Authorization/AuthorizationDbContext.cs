using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SHWDTech.IOT.Storage.Authorization
{
    public class AuthorizationDbContext : IdentityDbContext<SHWDIdentityUser>
    {
        public virtual IDbSet<Audience> Audiences { get; set; }

        public virtual IDbSet<SystemConfig> SystemConfigs { get; set; }
    }
}
