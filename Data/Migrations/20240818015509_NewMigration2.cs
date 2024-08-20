using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskWebSite.Data.Migrations
{
    public partial class NewMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InReplyTo",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "EmailMessages");

            migrationBuilder.DropColumn(
                name: "References",
                table: "EmailMessages");

            migrationBuilder.AddColumn<string>(
                name: "ConversationId",
                table: "EmailMessages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "EmailMessages");

            migrationBuilder.AddColumn<string>(
                name: "InReplyTo",
                table: "EmailMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "EmailMessages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "References",
                table: "EmailMessages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
