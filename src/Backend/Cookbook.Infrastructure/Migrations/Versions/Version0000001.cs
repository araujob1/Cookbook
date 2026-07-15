using FluentMigrator;

namespace Cookbook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.CREATE_TABLE_USER, "Create users table")]
public sealed class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTable("users")
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("email").AsString(255).NotNullable()
            .WithColumn("password_hash").AsString(2000).NotNullable();

        Execute.Sql("""
            CREATE UNIQUE INDEX ix_users_email
            ON users (email)
            WHERE is_active = TRUE
            """);
    }
}
