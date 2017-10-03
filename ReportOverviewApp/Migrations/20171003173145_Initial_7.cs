using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ReportOverviewApp.Migrations
{
    public partial class Initial_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClientNotified",
                table: "ReportDeadlines");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "ReportDeadlines");

            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "ReportDeadlines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClientNotified",
                table: "ReportDeadlines",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "ReportDeadlines",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "ReportDeadlines",
                nullable: false,
                defaultValue: false);
        }
    }
}
