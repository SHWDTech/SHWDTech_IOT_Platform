using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SHWDTech.IOT.Storage.Authorization
{
    public class AuthorizationDbContext : IdentityDbContext<SHWDIdentityUser>
    {
        private const string DefaultConnName = "SHWDAuthorization";

        public AuthorizationDbContext() : base(DefaultConnName)
        {
            
        }

        public AuthorizationDbContext(string connStr) : base(connStr)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SHWDIdentityUser>()
                .HasRequired(u => u.Audience)
                .WithMany(a => a.IdentityUsers);
            base.OnModelCreating(modelBuilder);
        }

        public virtual IDbSet<Audience> Audiences { get; set; }

        public virtual IDbSet<SystemConfig> SystemConfigs { get; set; }

        public virtual IDbSet<ServiceInvoker> ServiceInvokers { get; set; }
    }
}
