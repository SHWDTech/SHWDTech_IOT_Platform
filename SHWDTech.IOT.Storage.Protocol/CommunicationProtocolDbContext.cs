using System.Data.Entity;
using SHWDTech.IOT.Storage.Communication.Entities;

namespace SHWDTech.IOT.Storage.Communication
{
    public class CommunicationProtocolDbContext : DbContext
    {
        public CommunicationProtocolDbContext(string connStr) : base(connStr)
        {
            
        }

        public virtual DbSet<FirmwareSet> FirmwareSets { get; set; }

        public virtual DbSet<Firmware> Firmwares { get; set; }

        public virtual DbSet<Protocol> Protocols { get; set; }

        public virtual DbSet<ProtocolStructure> ProtocolStructures { get; set; }

        public virtual DbSet<ProtocolCommand> ProtocolCommands { get; set; }

        public virtual DbSet<CommandData> CommandDatas { get; set; }

        public virtual DbSet<Business> Businesses { get; set; }

        public virtual DbSet<Device> Devices { get; set; }

        public virtual DbSet<ProtocolData> ProtocolDatas { get; set; }

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
