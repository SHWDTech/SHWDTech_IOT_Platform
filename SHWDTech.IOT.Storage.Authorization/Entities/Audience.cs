using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Authorization.Entities
{
    public class Audience : DataItem<string>
    {
        [MaxLength(256)]
        [Required]
        public string Base64Secret { get; set; }

        [MaxLength(256)]
        [Required]
        public string Name { get; set; }

        [Required]
        public AudienceType AudienceType { get; set; }

        public virtual ICollection<SHWDIdentityUser> IdentityUsers { get; set; }
    }
}
