using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductMicroservice.Migrations
{
    public partial class ProductMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LinkImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "LinkImage", "Name" },
                values: new object[,]
                {
                    { new Guid("37ad541b-d101-46e6-89cf-07fe2848f3fa"), new DateTime(2022, 12, 16, 14, 32, 30, 180, DateTimeKind.Utc).AddTicks(1287), "https://pm1.narvii.com/6810/05dbd7aaebf3454313b99edfd566b06356a59be3v2_hq.jpg", "Cheese" },
                    { new Guid("438167bf-46cf-47b1-8ca8-307ff3299063"), new DateTime(2022, 12, 16, 14, 32, 30, 180, DateTimeKind.Utc).AddTicks(1288), "https://g.foolcdn.com/image/?url=https%3A//g.foolcdn.com/editorial/images/218648/eggs-brown-getty_BSCxkDW.jpg&w=2000&op=resize", "Egg" },
                    { new Guid("997d3739-f810-48a2-9363-d5bcddde1a10"), new DateTime(2022, 12, 16, 14, 32, 30, 180, DateTimeKind.Utc).AddTicks(1283), "https://craves.everybodyshops.com/wp-content/uploads/ThinkstockPhotos-535489242-1024x683@2x.jpg", "Milk" },
                    { new Guid("a6bea552-82df-4bd6-b372-df456969e59a"), new DateTime(2022, 12, 16, 14, 32, 30, 180, DateTimeKind.Utc).AddTicks(1286), "https://images5.alphacoders.com/102/1022723.jpg", "Juice" },
                    { new Guid("bba48c53-6c71-4924-98f1-f710d9baecf5"), new DateTime(2022, 12, 16, 14, 32, 30, 180, DateTimeKind.Utc).AddTicks(1285), "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg", "Bread" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
