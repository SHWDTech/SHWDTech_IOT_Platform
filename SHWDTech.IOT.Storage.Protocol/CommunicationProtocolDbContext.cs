using System.Data.Entity;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace SHWDTech.IOT.Storage.Communication
{
    public class CommunicationProtocolDbContext : DbContext
    {
        public CommunicationProtocolDbContext() : base("CommProtocol")
        {
            
        }

        public CommunicationProtocolDbContext(string connStr) : base(connStr)
        {
            
        }

        public virtual IDbSet<FirmwareSet> FirmwareSets { get; set; }

        public virtual IDbSet<Firmware> Firmwares { get; set; }

        public virtual IDbSet<Protocol> Protocols { get; set; }

        public virtual IDbSet<ProtocolStructure> ProtocolStructures { get; set; }

        public virtual IDbSet<ProtocolCommand> ProtocolCommands { get; set; }

        public virtual IDbSet<CommandData> CommandDatas { get; set; }

        public virtual IDbSet<Business> Businesses { get; set; }

        public virtual IDbSet<Device> Devices { get; set; }

        public virtual IDbSet<ProtocolData> ProtocolDatas { get; set; }

        public virtual IDbSet<SystemConfig> SystemConfigs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FirmwareSet>()
                .HasMany(s => s.Firmwares)
                .WithMany(c => c.FirmwareSets)
                .Map(cs =>
                {
                    cs.MapLeftKey("FirmwareId");
                    cs.MapRightKey("FirmwareSetId");
                    cs.ToTable("FirmwareSetFirmware");
                });

            modelBuilder.Entity<Protocol>()
                .HasMany(s => s.Firmwares)
                .WithMany(c => c.Protocols)
                .Map(cs =>
                {
                    cs.MapLeftKey("ProtocolId");
                    cs.MapRightKey("FirmwareId");
                    cs.ToTable("ProtocolFirmware");
                });
        }
    }
}
