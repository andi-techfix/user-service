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
                name: "subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SubscriptionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_SubscriptionId",
                table: "users",
                column: "SubscriptionId");
            
            migrationBuilder.Sql(@"
                INSERT INTO ""subscriptions"" (""Id"", ""Type"", ""StartDate"", ""EndDate"") VALUES
                  (1, 0, '2022-05-17 15:28:19', '2099-01-01 00:00:00'),
                  (2, 2, '2022-05-18 15:28:19', '2022-08-18 15:28:19'),
                  (3, 1, '2022-05-19 15:28:19', '2022-06-19 15:28:19'),
                  (4, 0, '2022-05-20 15:28:19', '2099-01-01 00:00:00'),
                  (5, 1, '2022-05-21 15:28:19', '2022-06-21 15:28:19'),
                  (6, 2, '2022-05-22 15:28:19', '2023-05-22 15:28:19'),
                  (7, 2, '2022-05-23 15:28:19', '2023-05-23 15:28:19')
                ON CONFLICT (""Id"") DO NOTHING;
                
                INSERT INTO ""users"" (""Id"", ""Name"", ""Email"", ""SubscriptionId"") VALUES
                  (1, 'John Doe',     'John@example.com',   2),
                  (2, 'Mark Shimko',  'Mark@example.com',   5),
                  (3, 'Taras Ovruch', 'Taras@example.com',  6)
                ON CONFLICT (""Id"") DO NOTHING;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM ""users"" WHERE ""Id"" IN (1,2,3);
                DELETE FROM ""subscriptions"" WHERE ""Id"" IN (1,2,3,4,5,6,7);
            ");
            
            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "subscriptions");
        }
    }
}
