using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ReportOverviewApp.Migrations
{
    public partial class Initial11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportDeadlines_Report_ReportId",
                table: "ReportDeadlines");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLog_AspNetUsers_UserId",
                table: "UserLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLog",
                table: "UserLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "BusinessContact",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "BusinessOwner",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "IsFromOtherDepartment",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Report");

            migrationBuilder.RenameTable(
                name: "UserLog",
                newName: "UserLogs");

            migrationBuilder.RenameTable(
                name: "Report",
                newName: "Reports");

            migrationBuilder.RenameIndex(
                name: "IX_UserLog_UserId",
                table: "UserLogs",
                newName: "IX_UserLogs_UserId");

            migrationBuilder.AddColumn<int>(
                name: "BusinessContactId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogs",
                table: "UserLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reports",
                table: "Reports",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BusinessContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BusinessOwner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalAbbreviation = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    WindwardId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plans_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportPlanMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportPlanMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportPlanMapping_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportPlanMapping_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_BusinessContactId",
                table: "Reports",
                column: "BusinessContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_StateId",
                table: "Plans",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPlanMapping_PlanId",
                table: "ReportPlanMapping",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportPlanMapping_ReportId",
                table: "ReportPlanMapping",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDeadlines_Reports_ReportId",
                table: "ReportDeadlines",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_BusinessContacts_BusinessContactId",
                table: "Reports",
                column: "BusinessContactId",
                principalTable: "BusinessContacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogs_AspNetUsers_UserId",
                table: "UserLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportDeadlines_Reports_ReportId",
                table: "ReportDeadlines");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_BusinessContacts_BusinessContactId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogs_AspNetUsers_UserId",
                table: "UserLogs");

            migrationBuilder.DropTable(
                name: "BusinessContacts");

            migrationBuilder.DropTable(
                name: "ReportPlanMapping");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogs",
                table: "UserLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reports",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_BusinessContactId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "BusinessContactId",
                table: "Reports");

            migrationBuilder.RenameTable(
                name: "UserLogs",
                newName: "UserLog");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Report");

            migrationBuilder.RenameIndex(
                name: "IX_UserLogs_UserId",
                table: "UserLog",
                newName: "IX_UserLog_UserId");

            migrationBuilder.AddColumn<string>(
                name: "BusinessContact",
                table: "Report",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessOwner",
                table: "Report",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "Report",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFromOtherDepartment",
                table: "Report",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Report",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLog",
                table: "UserLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportDeadlines_Report_ReportId",
                table: "ReportDeadlines",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLog_AspNetUsers_UserId",
                table: "UserLog",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
