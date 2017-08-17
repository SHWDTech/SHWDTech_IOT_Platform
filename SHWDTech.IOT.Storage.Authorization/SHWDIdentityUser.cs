using Microsoft.AspNet.Identity.EntityFramework;
// ReSharper disable InconsistentNaming

namespace SHWDTech.IOT.Storage.Authorization
{
    public class SHWDIdentityUser : IdentityUser
    {
        public virtual Audience Audience { get; set; }
    }
}
