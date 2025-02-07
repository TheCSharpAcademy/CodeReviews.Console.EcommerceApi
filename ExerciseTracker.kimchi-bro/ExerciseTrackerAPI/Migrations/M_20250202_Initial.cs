using FluentMigrator;

namespace ExerciseTrackerAPI.Migrations;

[Migration(20250202)]
public class M_20250202_Initial : Migration
{
    public override void Up()
    {
        Create.Table("ExerciseTypes")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable().Unique();

        Create.Table("Exercises")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("ExerciseTypeName").AsString(50).NotNullable()
            .WithColumn("StartTime").AsDateTime().Nullable()
            .WithColumn("EndTime").AsDateTime().Nullable()
            .WithColumn("Duration").AsTime().Nullable()
            .WithColumn("Comments").AsString(int.MaxValue).Nullable();

        Create.ForeignKey("FK_Exercises_ExerciseTypes")
            .FromTable("Exercises").ForeignColumn("ExerciseTypeName")
            .ToTable("ExerciseTypes").PrimaryColumn("Name")
            .OnDelete(System.Data.Rule.Cascade)
            .OnUpdate(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("Exercises");
        Delete.Table("ExerciseTypes");
    }
}
