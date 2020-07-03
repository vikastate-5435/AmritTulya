namespace AmritTulya.EntityLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NothingChange12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventories", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inventories", "ImagePath");
        }
    }
}
