namespace SHWDTech.IOT.Storage.Communication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterProtocolDataTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProtocolDatas", "Type", c => c.Byte(nullable: false));
            AlterColumn("dbo.CommandDatas", "DataName", c => c.String(unicode: false));
            AlterColumn("dbo.CommandDatas", "DisplayName", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.ProtocolCommands", "CommandCategory", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.ProtocolCommands", "DeliverParamString", c => c.String(nullable: false, unicode: false));
            AlterColumn("dbo.Protocols", "ReleaseDateTime", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.ProtocolDatas", "ProtocolId", c => c.Guid());
            AlterColumn("dbo.ProtocolDatas", "DecodeDateTime", c => c.DateTime(precision: 0));
            AlterColumn("dbo.ProtocolDatas", "UpdateDateTime", c => c.DateTime(nullable: false, precision: 0));
            CreateIndex("dbo.ProtocolDatas", new[] { "BusinessId", "Type", "DeviceId" }, clustered: true, name: "Ix_Business_Type_Device_UpdateTime");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProtocolDatas", "Ix_Business_Type_Device_UpdateTime");
            AlterColumn("dbo.ProtocolDatas", "UpdateDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ProtocolDatas", "DecodeDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ProtocolDatas", "ProtocolId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Protocols", "ReleaseDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ProtocolCommands", "DeliverParamString", c => c.String(nullable: false));
            AlterColumn("dbo.ProtocolCommands", "CommandCategory", c => c.String(nullable: false));
            AlterColumn("dbo.CommandDatas", "DisplayName", c => c.String(nullable: false));
            AlterColumn("dbo.CommandDatas", "DataName", c => c.String());
            DropColumn("dbo.ProtocolDatas", "Type");
            CreateIndex("dbo.ProtocolDatas", new[] { "DeviceId", "UpdateDateTime" }, clustered: true, name: "Ix_Device_UpdateTime");
        }
    }
}
