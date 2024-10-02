using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDeskWebSite.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Recipient = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyPlain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrippedText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrippedSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StrippedHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentCount = table.Column<int>(type: "int", nullable: true),
                    Timestamp = table.Column<int>(type: "int", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MessageHeaders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentIdMap = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    InReply = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListOfRequests",
                columns: table => new
                {
                    ListOfRequestsID = table.Column<int>(type: "int", nullable: false),
                    RequestID = table.Column<int>(type: "int", nullable: true),
                    HeadOfConversation = table.Column<int>(type: "int", nullable: true),
                    TailOfConversation = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListOfRequests", x => x.ListOfRequestsID);
                    table.ForeignKey(
                        name: "FK_ListOfRequests_EmailMessages_ListOfRequestsID",
                        column: x => x.ListOfRequestsID,
                        principalTable: "EmailMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListOfRequests");

            migrationBuilder.DropTable(
                name: "EmailMessages");
        }
    }
}
