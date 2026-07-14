using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Cookbook.Infrastructure.Migrations.Versions;

public abstract class VersionBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string tableName)
    {
        return Create.Table(tableName)
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("is_active").AsBoolean().NotNullable();
    }
}
