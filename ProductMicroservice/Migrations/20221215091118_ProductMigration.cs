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
                    { new Guid("4dabfde6-b155-4e93-a510-c8fdd502ef83"), new DateTime(2022, 12, 15, 9, 11, 18, 515, DateTimeKind.Utc).AddTicks(7249), "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg", "Bread" },
                    { new Guid("62e23a7a-b659-4a9f-a417-3355ae4b1d80"), new DateTime(2022, 12, 15, 9, 11, 18, 515, DateTimeKind.Utc).AddTicks(7251), "https://pm1.narvii.com/6810/05dbd7aaebf3454313b99edfd566b06356a59be3v2_hq.jpg", "Cheese" },
                    { new Guid("67784688-8ca1-4e6a-a773-2831db0e16dc"), new DateTime(2022, 12, 15, 9, 11, 18, 515, DateTimeKind.Utc).AddTicks(7250), "https://images5.alphacoders.com/102/1022723.jpg", "Juice" },
                    { new Guid("ab8a02d5-95d2-4c7b-b1e8-d70a048d98e2"), new DateTime(2022, 12, 15, 9, 11, 18, 515, DateTimeKind.Utc).AddTicks(7243), "https://craves.everybodyshops.com/wp-content/uploads/ThinkstockPhotos-535489242-1024x683@2x.jpg", "Milk" },
                    { new Guid("aed0c7d8-18ed-4945-9d8b-cc3a0e7a92be"), new DateTime(2022, 12, 15, 9, 11, 18, 515, DateTimeKind.Utc).AddTicks(7252), "https://g.foolcdn.com/image/?url=https%3A//g.foolcdn.com/editorial/images/218648/eggs-brown-getty_BSCxkDW.jpg&w=2000&op=resize", "Egg" }
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
