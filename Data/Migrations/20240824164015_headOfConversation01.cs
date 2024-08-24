using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskWebSite.Data.Migrations
{
    public partial class headOfConversation01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ListOfRequests",
                columns: table => new
                {
                    HeadOfConversation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfRequests", x => x.HeadOfConversation);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListOfRequests");
        }
    }
}
