using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductMicroservice.Migrations
{
    public partial class FridgeDb : Migration
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
                    { new Guid("0d8e0c4f-d158-49c0-aab4-4595e23d4c33"), new DateTime(2022, 10, 2, 10, 18, 58, 28, DateTimeKind.Utc).AddTicks(5408), "https://www.expatica.com/app/uploads/sites/2/2014/05/bread.jpg", "Bread" },
                    { new Guid("c5fbace4-df51-49de-a599-66d369011b20"), new DateTime(2022, 10, 2, 10, 18, 58, 28, DateTimeKind.Utc).AddTicks(5412), "https://g.foolcdn.com/image/?url=https%3A//g.foolcdn.com/editorial/images/218648/eggs-brown-getty_BSCxkDW.jpg&w=2000&op=resize", "Egg" },
                    { new Guid("d06ea4d3-88ae-4316-84aa-210e5abcb553"), new DateTime(2022, 10, 2, 10, 18, 58, 28, DateTimeKind.Utc).AddTicks(5410), "https://pm1.narvii.com/6810/05dbd7aaebf3454313b99edfd566b06356a59be3v2_hq.jpg", "Cheese" },
                    { new Guid("f47bd05d-53f0-4857-93d4-2e8dd396c09a"), new DateTime(2022, 10, 2, 10, 18, 58, 28, DateTimeKind.Utc).AddTicks(5406), "https://craves.everybodyshops.com/wp-content/uploads/ThinkstockPhotos-535489242-1024x683@2x.jpg", "Milk" },
                    { new Guid("fd33edbb-fb43-433c-865d-6402e01654d3"), new DateTime(2022, 10, 2, 10, 18, 58, 28, DateTimeKind.Utc).AddTicks(5409), "https://images5.alphacoders.com/102/1022723.jpg", "Juice" }
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
