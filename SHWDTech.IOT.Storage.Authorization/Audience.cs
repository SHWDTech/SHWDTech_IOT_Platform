using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Authorization
{
    public class Audience : IDataItem<string>
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(256)]
        [Required]
        public string Base64Secret { get; set; }

        [MaxLength(256)]
        [Required]
        public string Name { get; set; }

        [MaxLength(256)]
        public string AppSecret { get; set; }

        [Required]
        public AudienceType AudienceType { get; set; }

        public virtual ICollection<SHWDIdentityUser> IdentityUsers { get; set; }
    }
}
