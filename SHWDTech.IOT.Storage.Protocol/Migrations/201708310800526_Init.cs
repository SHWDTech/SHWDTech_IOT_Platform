namespace SHWDTech.IOT.Storage.Communication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Businesses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BusinessName = c.String(maxLength: 200),
                        Port = c.Int(nullable: false),
                        StorageModule = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommandDatas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DataIndex = c.Int(nullable: false),
                        DataLength = c.Int(nullable: false),
                        DataName = c.String(),
                        DisplayName = c.String(nullable: false),
                        DataIndexFlag = c.Byte(nullable: false),
                        ValidFlagIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProtocolCommands",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProtocolId = c.Guid(nullable: false),
                        CommandCode = c.Binary(nullable: false, maxLength: 16),
                        SendBytesLength = c.Int(nullable: false),
                        ReceiveBytesLength = c.Int(nullable: false),
                        MaxReceiveBytesLength = c.Int(nullable: false),
                        CommandCategory = c.String(nullable: false),
                        DataOrderType = c.Byte(nullable: false),
                        DeliverParamString = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Protocols", t => t.ProtocolId, cascadeDelete: true)
                .Index(t => t.ProtocolId);
            
            CreateTable(
                "dbo.Protocols",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProtocolName = c.String(nullable: false, maxLength: 200),
                        ProtocolModule = c.String(nullable: false, maxLength: 200),
                        Version = c.String(nullable: false, maxLength: 100),
                        Head = c.Binary(nullable: false, maxLength: 10),
                        Tail = c.Binary(nullable: false, maxLength: 10),
                        ReleaseDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Firmwares",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirmwareName = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FirmwareSets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirmwareSetName = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProtocolStructures",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProtocolId = c.Guid(nullable: false),
                        StructureName = c.String(nullable: false, maxLength: 50),
                        StructureIndex = c.Int(nullable: false),
                        StructureDataLength = c.Int(nullable: false),
                        DefaultBytes = c.Binary(maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Protocols", t => t.ProtocolId, cascadeDelete: true)
                .Index(t => t.ProtocolId);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BusinessId = c.Guid(nullable: false),
                        DeviceName = c.String(nullable: false, maxLength: 100),
                        NodeId = c.Binary(nullable: false, maxLength: 16),
                        CreditCode = c.Binary(maxLength: 512),
                        EncryptType = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.BusinessId, cascadeDelete: true)
                .Index(t => new { t.BusinessId, t.NodeId }, unique: true, name: "Ix_Business_DeviceNodeId_Unique");
            
            CreateTable(
                "dbo.ProtocolDatas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BusinessId = c.Guid(nullable: false),
                        DeviceId = c.Long(nullable: false),
                        ProtocolContent = c.Binary(nullable: false),
                        ContentLength = c.Int(nullable: false),
                        ProtocolId = c.Guid(nullable: false),
                        DecodeDateTime = c.DateTime(nullable: false),
                        UpdateDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.BusinessId, cascadeDelete: true)
                .ForeignKey("dbo.Devices", t => t.DeviceId, cascadeDelete: true)
                .ForeignKey("dbo.Protocols", t => t.ProtocolId, cascadeDelete: true)
                .Index(t => t.BusinessId)
                .Index(t => t.DeviceId)
                .Index(t => t.ProtocolId)
                .Index(t => t.UpdateDateTime, clustered: true, name: "Ix_Device_UpdateTime");
            
            CreateTable(
                "dbo.SystemConfigs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        BusinessId = c.Guid(),
                        ItemType = c.String(nullable: false, maxLength: 256),
                        ItemKey = c.String(nullable: false, maxLength: 512),
                        ItemValue = c.String(nullable: false, maxLength: 2048),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProtocolCommandCommandDatas",
                c => new
                    {
                        ProtocolCommand_Id = c.Guid(nullable: false),
                        CommandData_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProtocolCommand_Id, t.CommandData_Id })
                .ForeignKey("dbo.ProtocolCommands", t => t.ProtocolCommand_Id, cascadeDelete: true)
                .ForeignKey("dbo.CommandDatas", t => t.CommandData_Id, cascadeDelete: true)
                .Index(t => t.ProtocolCommand_Id)
                .Index(t => t.CommandData_Id);
            
            CreateTable(
                "dbo.FirmwareSetFirmware",
                c => new
                    {
                        FirmwareId = c.Guid(nullable: false),
                        FirmwareSetId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.FirmwareId, t.FirmwareSetId })
                .ForeignKey("dbo.FirmwareSets", t => t.FirmwareId, cascadeDelete: true)
                .ForeignKey("dbo.Firmwares", t => t.FirmwareSetId, cascadeDelete: true)
                .Index(t => t.FirmwareId)
                .Index(t => t.FirmwareSetId);
            
            CreateTable(
                "dbo.ProtocolFirmware",
                c => new
                    {
                        ProtocolId = c.Guid(nullable: false),
                        FirmwareId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProtocolId, t.FirmwareId })
                .ForeignKey("dbo.Protocols", t => t.ProtocolId, cascadeDelete: true)
                .ForeignKey("dbo.Firmwares", t => t.FirmwareId, cascadeDelete: true)
                .Index(t => t.ProtocolId)
                .Index(t => t.FirmwareId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProtocolDatas", "ProtocolId", "dbo.Protocols");
            DropForeignKey("dbo.ProtocolDatas", "DeviceId", "dbo.Devices");
            DropForeignKey("dbo.ProtocolDatas", "BusinessId", "dbo.Businesses");
            DropForeignKey("dbo.Devices", "BusinessId", "dbo.Businesses");
            DropForeignKey("dbo.ProtocolStructures", "ProtocolId", "dbo.Protocols");
            DropForeignKey("dbo.ProtocolCommands", "ProtocolId", "dbo.Protocols");
            DropForeignKey("dbo.ProtocolFirmware", "FirmwareId", "dbo.Firmwares");
            DropForeignKey("dbo.ProtocolFirmware", "ProtocolId", "dbo.Protocols");
            DropForeignKey("dbo.FirmwareSetFirmware", "FirmwareSetId", "dbo.Firmwares");
            DropForeignKey("dbo.FirmwareSetFirmware", "FirmwareId", "dbo.FirmwareSets");
            DropForeignKey("dbo.ProtocolCommandCommandDatas", "CommandData_Id", "dbo.CommandDatas");
            DropForeignKey("dbo.ProtocolCommandCommandDatas", "ProtocolCommand_Id", "dbo.ProtocolCommands");
            DropIndex("dbo.ProtocolFirmware", new[] { "FirmwareId" });
            DropIndex("dbo.ProtocolFirmware", new[] { "ProtocolId" });
            DropIndex("dbo.FirmwareSetFirmware", new[] { "FirmwareSetId" });
            DropIndex("dbo.FirmwareSetFirmware", new[] { "FirmwareId" });
            DropIndex("dbo.ProtocolCommandCommandDatas", new[] { "CommandData_Id" });
            DropIndex("dbo.ProtocolCommandCommandDatas", new[] { "ProtocolCommand_Id" });
            DropIndex("dbo.ProtocolDatas", "Ix_Device_UpdateTime");
            DropIndex("dbo.ProtocolDatas", new[] { "ProtocolId" });
            DropIndex("dbo.ProtocolDatas", new[] { "DeviceId" });
            DropIndex("dbo.ProtocolDatas", new[] { "BusinessId" });
            DropIndex("dbo.Devices", "Ix_Business_DeviceNodeId_Unique");
            DropIndex("dbo.ProtocolStructures", new[] { "ProtocolId" });
            DropIndex("dbo.ProtocolCommands", new[] { "ProtocolId" });
            DropTable("dbo.ProtocolFirmware");
            DropTable("dbo.FirmwareSetFirmware");
            DropTable("dbo.ProtocolCommandCommandDatas");
            DropTable("dbo.SystemConfigs");
            DropTable("dbo.ProtocolDatas");
            DropTable("dbo.Devices");
            DropTable("dbo.ProtocolStructures");
            DropTable("dbo.FirmwareSets");
            DropTable("dbo.Firmwares");
            DropTable("dbo.Protocols");
            DropTable("dbo.ProtocolCommands");
            DropTable("dbo.CommandDatas");
            DropTable("dbo.Businesses");
        }
    }
}
