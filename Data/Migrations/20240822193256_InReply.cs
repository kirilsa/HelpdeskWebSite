using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskWebSite.Data.Migrations
{
    public partial class InReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InReply",
                table: "EmailMessages",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InReply",
                table: "EmailMessages");
        }
    }
}
