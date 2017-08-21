using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Communication.Entities
{
    public class Business : DataItem<Guid>
    {
        [MaxLength(200)]
        public string BusinessName { get; set; }

        public int Port { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(100)]
        public string StorageModule { get; set; }
    }
}
