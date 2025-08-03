using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialTablesCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValueSql: "NOW()"),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.CheckConstraint("CK_Subscriptions_EndDate_After_StartDate", "\"EndDate\" > \"StartDate\"");
                    table.CheckConstraint("CK_Subscriptions_EndDate_In_Future", "\"EndDate\" > NOW()");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SubscriptionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubscriptionId",
                table: "Users",
                column: "SubscriptionId");

            migrationBuilder.Sql(@"
                INSERT INTO ""Subscriptions"" (""Id"", ""Type"", ""StartDate"", ""EndDate"") VALUES
                  (1, 0, '2026-05-17 15:28:19', '2099-01-01 00:00:00'),
                  (2, 2, '2022-05-18 15:28:19', '2026-08-18 15:28:19'),
                  (3, 1, '2022-05-19 15:28:19', '2026-06-19 15:28:19'),
                  (4, 0, '2022-05-20 15:28:19', '2096-01-01 00:00:00'),
                  (5, 1, '2022-05-21 15:28:19', '2026-06-21 15:28:19'),
                  (6, 2, '2022-05-22 15:28:19', '2026-05-22 15:28:19'),
                  (7, 2, '2022-05-23 15:28:19', '2026-05-23 15:28:19')
                ON CONFLICT (""Id"") DO NOTHING;
                
                INSERT INTO ""Users"" (""Id"", ""Name"", ""Email"", ""SubscriptionId"") VALUES
                  (1, 'John Doe',     'John@example.com',   2),
                  (2, 'Mark Shimko',  'Mark@example.com',   5),
                  (3, 'Taras Ovruch', 'Taras@example.com',  6)
                ON CONFLICT (""Id"") DO NOTHING;
            ");

            migrationBuilder.Sql(@"
                SELECT setval(
                    pg_get_serial_sequence('""Subscriptions""', 'Id'),
                    (SELECT COALESCE(MAX(""Id""), 1) FROM ""Subscriptions"")
                );
            ");

            migrationBuilder.Sql(@"
                SELECT setval(
                    pg_get_serial_sequence('""Users""', 'Id'),
                    (SELECT COALESCE(MAX(""Id""), 1) FROM ""Users"")
                );
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM ""Users"" WHERE ""Id"" IN (1,2,3);
                DELETE FROM ""Subscriptions"" WHERE ""Id"" IN (1,2,3,4,5,6,7);
            ");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Subscriptions");
        }
    }
}
