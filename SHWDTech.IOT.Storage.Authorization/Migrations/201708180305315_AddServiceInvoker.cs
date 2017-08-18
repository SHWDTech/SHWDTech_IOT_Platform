namespace SHWDTech.IOT.Storage.Authorization.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddServiceInvoker : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceInvokers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(unicode: false),
                        SecurityStamp = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ServiceInvokers");
        }
    }
}
