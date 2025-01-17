using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace IngrEasy.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{

    public ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
      return  Create.Table(table)
            .WithColumn("CreatedOn").AsDateTime().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable()
            .WithColumn("Id").AsInt32().PrimaryKey().Identity();
    }
}