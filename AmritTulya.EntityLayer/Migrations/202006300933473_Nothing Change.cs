namespace AmritTulya.EntityLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NothingChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersEntities", "userRole", c => c.String());
            AlterColumn("dbo.UsersEntities", "Username", c => c.String(nullable: false));
            AlterColumn("dbo.UsersEntities", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UsersEntities", "Password", c => c.String());
            AlterColumn("dbo.UsersEntities", "Username", c => c.String());
            DropColumn("dbo.UsersEntities", "userRole");
        }
    }
}
