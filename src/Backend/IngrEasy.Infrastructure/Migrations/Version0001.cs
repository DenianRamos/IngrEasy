using FluentMigrator;
using IngrEasy.Infrastructure.Migrations.Versions;

namespace IngrEasy.Infrastructure.Migrations;
[Migration(DatabaseVersion.TableUser, "Create the User table")]

public class Version0001 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable();
    }
}