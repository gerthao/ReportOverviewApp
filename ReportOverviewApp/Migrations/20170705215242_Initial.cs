using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportOverviewApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrequencyType",
                table: "Report");

            migrationBuilder.AddColumn<int>(
                name: "Frequency",
                table: "Report",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Report");

            migrationBuilder.AddColumn<int>(
                name: "FrequencyType",
                table: "Report",
                nullable: false,
                defaultValue: 0);
        }
    }
}
