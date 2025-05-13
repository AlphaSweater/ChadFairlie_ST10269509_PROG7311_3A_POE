using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBL_Employees",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    first_name = table.Column<string>(type: "TEXT", maxLength: 55, nullable: false),
                    last_name = table.Column<string>(type: "TEXT", maxLength: 55, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", nullable: false),
                    created_on = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_on = table.Column<DateTime>(type: "TEXT", nullable: true),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_Employees", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "TBL_Farmers",
                columns: table => new
                {
                    farmer_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    first_name = table.Column<string>(type: "TEXT", maxLength: 55, nullable: false),
                    last_name = table.Column<string>(type: "TEXT", maxLength: 55, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", nullable: false),
                    created_on = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_on = table.Column<DateTime>(type: "TEXT", nullable: true),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_by_employee_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_Farmers", x => x.farmer_id);
                    table.ForeignKey(
                        name: "FK_TBL_Farmers_TBL_Employees_created_by_employee_id",
                        column: x => x.created_by_employee_id,
                        principalTable: "TBL_Employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TBL_Products",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    price = table.Column<double>(type: "REAL", nullable: false),
                    image = table.Column<byte[]>(type: "BLOB", nullable: true),
                    farmer_id = table.Column<int>(type: "INTEGER", nullable: false),
                    created_on = table.Column<DateTime>(type: "TEXT", nullable: true),
                    updated_on = table.Column<DateTime>(type: "TEXT", nullable: true),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_Products", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_TBL_Products_TBL_Farmers_farmer_id",
                        column: x => x.farmer_id,
                        principalTable: "TBL_Farmers",
                        principalColumn: "farmer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBL_Farmers_created_by_employee_id",
                table: "TBL_Farmers",
                column: "created_by_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_TBL_Products_farmer_id",
                table: "TBL_Products",
                column: "farmer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_Products");

            migrationBuilder.DropTable(
                name: "TBL_Farmers");

            migrationBuilder.DropTable(
                name: "TBL_Employees");
        }
    }
}
