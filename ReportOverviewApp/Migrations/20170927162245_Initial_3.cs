using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ReportOverviewApp.Migrations
{
    public partial class Initial_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientNotified",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "DateClientNotified",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "DateDone",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "DateSent",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "Done",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "OtherDepartment",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "QualityIndicator",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "Sent",
                table: "Report");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClientNotifiedDate",
                table: "Report",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedDate",
                table: "Report",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClientNotified",
                table: "Report",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Report",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFromOtherDepartment",
                table: "Report",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsQualityIndicator",
                table: "Report",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "Report",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentDate",
                table: "Report",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IsFromOtherDepartment",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "IsQualityIndicator",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "SentDate",
                table: "Report");

            migrationBuilder.AddColumn<bool>(
                name: "ClientNotified",
                table: "Report",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateClientNotified",
                table: "Report",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDone",
                table: "Report",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSent",
                table: "Report",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "Report",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OtherDepartment",
                table: "Report",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QualityIndicator",
                table: "Report",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Sent",
                table: "Report",
                nullable: false,
                defaultValue: false);
        }
    }
}
