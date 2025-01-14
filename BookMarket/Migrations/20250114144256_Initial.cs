using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMarket.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Author = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 255, nullable: false),
                    Title = table.Column<string>(type: "varchar(1024)", unicode: false, maxLength: 1023, nullable: false),
                    YearPublication = table.Column<int>(type: "integer", unicode: false, nullable: false),
                    Quantity = table.Column<int>(type: "integer", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
