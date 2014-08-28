namespace ExpressQuiz.Core.Migrations.Quiz
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        Text = c.String(nullable: false, maxLength: 500),
                        Explanation = c.String(),
                        IsCorrect = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Question", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 1000),
                        OrderId = c.Int(nullable: false),
                        EstimatedTime = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        QuizId = c.Int(nullable: false),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quiz", t => t.QuizId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionType", t => t.Type_Id)
                .Index(t => t.QuizId)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.Quiz",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Summary = c.String(nullable: false, maxLength: 1000),
                        IsTimeable = c.Boolean(nullable: false),
                        AllowPoints = c.Boolean(nullable: false),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.String(nullable: false),
                        Locked = c.Boolean(nullable: false),
                        QuizCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuizCategory", t => t.QuizCategoryId, cascadeDelete: true)
                .Index(t => t.QuizCategoryId);
            
            CreateTable(
                "dbo.QuizCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "NameIndex");
            
            CreateTable(
                "dbo.QuestionType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContactInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false),
                        Message = c.String(nullable: false, maxLength: 1000),
                        Created = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuizRating",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuizId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuizResult",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuizId = c.Int(nullable: false),
                        EllapsedTime = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Quiz", t => t.QuizId, cascadeDelete: true)
                .Index(t => t.QuizId);
            
            CreateTable(
                "dbo.UserAnswer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnswerId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        QuizResultId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuizResult", t => t.QuizResultId, cascadeDelete: true)
                .Index(t => t.QuizResultId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuizResult", "QuizId", "dbo.Quiz");
            DropForeignKey("dbo.UserAnswer", "QuizResultId", "dbo.QuizResult");
            DropForeignKey("dbo.Question", "Type_Id", "dbo.QuestionType");
            DropForeignKey("dbo.Question", "QuizId", "dbo.Quiz");
            DropForeignKey("dbo.Quiz", "QuizCategoryId", "dbo.QuizCategory");
            DropForeignKey("dbo.Answer", "QuestionId", "dbo.Question");
            DropIndex("dbo.UserAnswer", new[] { "QuizResultId" });
            DropIndex("dbo.QuizResult", new[] { "QuizId" });
            DropIndex("dbo.QuizCategory", "NameIndex");
            DropIndex("dbo.Quiz", new[] { "QuizCategoryId" });
            DropIndex("dbo.Question", new[] { "Type_Id" });
            DropIndex("dbo.Question", new[] { "QuizId" });
            DropIndex("dbo.Answer", new[] { "QuestionId" });
            DropTable("dbo.UserAnswer");
            DropTable("dbo.QuizResult");
            DropTable("dbo.QuizRating");
            DropTable("dbo.ContactInfo");
            DropTable("dbo.QuestionType");
            DropTable("dbo.QuizCategory");
            DropTable("dbo.Quiz");
            DropTable("dbo.Question");
            DropTable("dbo.Answer");
        }
    }
}
