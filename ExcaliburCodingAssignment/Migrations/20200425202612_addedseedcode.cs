using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExcaliburCodingAssignment.Migrations
{
    public partial class addedseedcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderDate",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDate", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "OrderCombined",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderAmount = table.Column<double>(type: "float", nullable: false),
                    OrderDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCombined", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCombined_OrderDate_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrderDate",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderAmount = table.Column<double>(type: "float", nullable: false),
                    OrderDesc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetail_OrderDate_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrderDate",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OrderDate",
                columns: new[] { "OrderID", "OrderedDate" },
                values: new object[,]
                {
                    { 20, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, new DateTime(2020, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, new DateTime(2020, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "OrderDetail",
                columns: new[] { "Id", "OrderAmount", "OrderDesc", "OrderId" },
                values: new object[,]
                {
                    { 20, 2000.0, "Order 2000", 20 },
                    { 21, 2100.0, "Order 2100", 21 },
                    { 22, 2200.0, "Order 2200", 22 },
                    { 23, 2300.0, "Order 2300", 23 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderCombined_OrderId",
                table: "OrderCombined",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderCombined");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "OrderDate");
        }
    }
}
