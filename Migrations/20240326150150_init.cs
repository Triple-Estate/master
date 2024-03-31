using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace G8CozyMVC.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Xp = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    APIKEY = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "agentuser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agentuser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "buy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    YearBuilt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SquareFeet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room = table.Column<short>(type: "smallint", nullable: false),
                    Bath = table.Column<short>(type: "smallint", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSell = table.Column<bool>(type: "bit", nullable: false),
                    HavePool = table.Column<bool>(type: "bit", nullable: false),
                    HaveOnSiteParking = table.Column<bool>(type: "bit", nullable: false),
                    HavePark = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "home",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    YearBuilt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SquareFeet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Room = table.Column<short>(type: "smallint", nullable: false),
                    Bath = table.Column<short>(type: "smallint", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSell = table.Column<bool>(type: "bit", nullable: false),
                    HavePool = table.Column<bool>(type: "bit", nullable: false),
                    HaveOnSiteParking = table.Column<bool>(type: "bit", nullable: false),
                    HavePark = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_home", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Msg = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agent");

            migrationBuilder.DropTable(
                name: "agentuser");

            migrationBuilder.DropTable(
                name: "buy");

            migrationBuilder.DropTable(
                name: "home");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
