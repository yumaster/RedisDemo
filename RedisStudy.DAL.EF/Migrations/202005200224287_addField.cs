namespace RedisStudy.DAL.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Function", "FunRemark", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Function", "FunRemark");
        }
    }
}
