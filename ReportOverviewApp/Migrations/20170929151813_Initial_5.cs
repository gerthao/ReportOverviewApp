using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ReportOverviewApp.Migrations
{
    public partial class Initial_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientNotifiedDate",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "FinishedDate",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "IsClientNotified",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "Report");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClientNotifiedDate",
                table: "Report",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedDate",
                table: "Report",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClientNotified",
                table: "Report",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Report",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "Report",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "Report",
                nullable: true);
        }
    }
}
