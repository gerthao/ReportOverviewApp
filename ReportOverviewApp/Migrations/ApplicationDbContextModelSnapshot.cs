﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ReportOverviewApp.Data;
using ReportOverviewApp.Models;
using System;

namespace ReportOverviewApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("Theme");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.BusinessContact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BusinessOwner")
                        .HasMaxLength(64);

                    b.Property<string>("Email")
                        .HasMaxLength(64);

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("BusinessContacts");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.Property<int>("StateId");

                    b.Property<string>("WindwardId")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BusinessContactId");

                    b.Property<DateTime?>("DateAdded");

                    b.Property<string>("DayDue")
                        .HasMaxLength(10);

                    b.Property<int?>("DaysAfterQuarter");

                    b.Property<string>("DeliverTo");

                    b.Property<string>("DeliveryFunction")
                        .HasMaxLength(1000);

                    b.Property<string>("DeliveryMethod")
                        .HasMaxLength(260);

                    b.Property<DateTime?>("DueDate1");

                    b.Property<DateTime?>("DueDate2");

                    b.Property<DateTime?>("DueDate3");

                    b.Property<DateTime?>("DueDate4");

                    b.Property<int?>("ERRStatus");

                    b.Property<string>("ERSReportLocation")
                        .HasMaxLength(4000);

                    b.Property<string>("ERSReportName")
                        .HasMaxLength(1000);

                    b.Property<DateTime?>("EffectiveDate");

                    b.Property<string>("FolderLocation")
                        .HasMaxLength(2000);

                    b.Property<string>("Frequency")
                        .HasMaxLength(50);

                    b.Property<bool>("IsQualityIndicator");

                    b.Property<int?>("LegacyReportID");

                    b.Property<int?>("LegacyReportIDR2");

                    b.Property<string>("Name")
                        .HasMaxLength(1000);

                    b.Property<string>("Notes");

                    b.Property<string>("OtherReportLocation")
                        .HasMaxLength(4000);

                    b.Property<string>("OtherReportName")
                        .HasMaxLength(1000);

                    b.Property<string>("ReportPath")
                        .HasMaxLength(2000);

                    b.Property<string>("ReportType")
                        .HasMaxLength(50);

                    b.Property<string>("RunWith")
                        .HasMaxLength(100);

                    b.Property<string>("SourceDepartment")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("SystemRefreshDate");

                    b.Property<DateTime?>("TerminationDate");

                    b.Property<string>("WorkInstructions");

                    b.HasKey("Id");

                    b.HasIndex("BusinessContactId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.ReportDeadline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ClientNotifiedDate");

                    b.Property<DateTime>("Deadline");

                    b.Property<DateTime?>("FinishedDate");

                    b.Property<int>("ReportId");

                    b.Property<DateTime?>("SentDate");

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("ReportDeadlines");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.ReportPlanMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PlanId");

                    b.Property<int>("ReportId");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.HasIndex("ReportId");

                    b.ToTable("ReportPlanMapping");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(64);

                    b.Property<string>("PostalAbbreviation")
                        .HasMaxLength(32);

                    b.Property<string>("Type")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.UserLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<string>("Notes");

                    b.Property<DateTime?>("TimeStamp");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogs");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ReportOverviewApp.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ReportOverviewApp.Models.Plan", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.State", "State")
                        .WithMany("Plans")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ReportOverviewApp.Models.Report", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.BusinessContact", "BusinessContact")
                        .WithMany("Reports")
                        .HasForeignKey("BusinessContactId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ReportOverviewApp.Models.ReportDeadline", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.Report", "Report")
                        .WithMany("Deadlines")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ReportOverviewApp.Models.ReportPlanMap", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.Plan", "Plan")
                        .WithMany("ReportPlanMapping")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ReportOverviewApp.Models.Report", "Report")
                        .WithMany("ReportPlanMapping")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ReportOverviewApp.Models.UserLog", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.ApplicationUser", "User")
                        .WithMany("UserLogs")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
