using System;
using System.ComponentModel.DataAnnotations;
using SHWDTech.IOT.Storage.Convention;

namespace SHWDTech.IOT.Storage.Authorization
{
    public class ServiceInvoker : IDataItem<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SecurityStamp { get; set; }
    }
}
