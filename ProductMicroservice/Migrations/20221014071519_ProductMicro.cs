using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductMicroservice.Migrations
{
    public partial class ProductMicro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    { new Guid("02f0ccb6-bff0-4b77-a1a5-1c6e331bb89b"), new DateTime(2022, 10, 14, 7, 15, 18, 885, DateTimeKind.Utc).AddTicks(6482), "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg", "Bread" },
                    { new Guid("67408836-035b-4fe3-b877-991a8d2f3560"), new DateTime(2022, 10, 14, 7, 15, 18, 885, DateTimeKind.Utc).AddTicks(6485), "https://g.foolcdn.com/image/?url=https%3A//g.foolcdn.com/editorial/images/218648/eggs-brown-getty_BSCxkDW.jpg&w=2000&op=resize", "Egg" },
                    { new Guid("8bac0be8-4af8-475a-b1eb-99bca1cc9b5f"), new DateTime(2022, 10, 14, 7, 15, 18, 885, DateTimeKind.Utc).AddTicks(6483), "https://images5.alphacoders.com/102/1022723.jpg", "Juice" },
                    { new Guid("96069b0b-b9ff-491d-8f8c-bdf159753e0c"), new DateTime(2022, 10, 14, 7, 15, 18, 885, DateTimeKind.Utc).AddTicks(6473), "https://craves.everybodyshops.com/wp-content/uploads/ThinkstockPhotos-535489242-1024x683@2x.jpg", "Milk" },
                    { new Guid("a9ba8e48-cd41-4b8d-acd0-fb8d257b24bc"), new DateTime(2022, 10, 14, 7, 15, 18, 885, DateTimeKind.Utc).AddTicks(6484), "https://pm1.narvii.com/6810/05dbd7aaebf3454313b99edfd566b06356a59be3v2_hq.jpg", "Cheese" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
