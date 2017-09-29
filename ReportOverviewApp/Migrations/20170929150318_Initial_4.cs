using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ReportOverviewApp.Migrations
{
    public partial class Initial_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Changes",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "ReportID",
                table: "UserLog");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserLog",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "UserLog",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Report",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserLog",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "UserLog",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReportDeadlines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientNotifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsClientNotified = table.Column<bool>(type: "bit", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportDeadlines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportDeadlines_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLog_UserId",
                table: "UserLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDeadlines_ReportId",
                table: "ReportDeadlines",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLog_AspNetUsers_UserId",
                table: "UserLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLog_AspNetUsers_UserId",
                table: "UserLog");

            migrationBuilder.DropTable(
                name: "ReportDeadlines");

            migrationBuilder.DropIndex(
                name: "IX_UserLog_UserId",
                table: "UserLog");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "UserLog");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserLog",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserLog",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Report",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "UserLog",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Changes",
                table: "UserLog",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportID",
                table: "UserLog",
                nullable: false,
                defaultValue: 0);
        }
    }
}
