namespace ExpressQuiz.Core.Migrations.Quiz
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moretext : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Answer", "Text", c => c.String(nullable: false, maxLength: 2000));
            AlterColumn("dbo.Question", "Text", c => c.String(nullable: false));
            AlterColumn("dbo.Quiz", "Summary", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Quiz", "Summary", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Question", "Text", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.Answer", "Text", c => c.String(nullable: false, maxLength: 500));
        }
    }
}
