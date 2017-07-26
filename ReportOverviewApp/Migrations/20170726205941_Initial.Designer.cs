using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ReportOverviewApp.Data;

namespace ReportOverviewApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170726205941_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
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
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
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

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.Report", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BusinessContact")
                        .HasMaxLength(255);

                    b.Property<string>("BusinessOwner")
                        .HasMaxLength(255);

                    b.Property<bool>("ClientNotified");

                    b.Property<DateTime?>("DateAdded");

                    b.Property<DateTime?>("DateClientNotified");

                    b.Property<DateTime?>("DateDone");

                    b.Property<DateTime?>("DateDue");

                    b.Property<DateTime?>("DateSent");

                    b.Property<string>("DayDue")
                        .HasMaxLength(10);

                    b.Property<int?>("DaysAfterQuarter")
                        .HasMaxLength(20);

                    b.Property<string>("DeliveryFunction")
                        .HasMaxLength(1000);

                    b.Property<string>("DeliveryMethod")
                        .HasMaxLength(260);

                    b.Property<string>("DeliveryTo");

                    b.Property<bool>("Done");

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

                    b.Property<string>("GroupName")
                        .HasMaxLength(255);

                    b.Property<int?>("LegacyReportID");

                    b.Property<int?>("LegacyReportIDR2");

                    b.Property<string>("Name")
                        .HasMaxLength(1000);

                    b.Property<string>("Notes");

                    b.Property<bool>("OtherDepartment");

                    b.Property<string>("OtherReportLocation")
                        .HasMaxLength(4000);

                    b.Property<string>("OtherReportName")
                        .HasMaxLength(1000);

                    b.Property<bool>("QualityIndicator");

                    b.Property<string>("ReportPath")
                        .HasMaxLength(2000);

                    b.Property<string>("ReportType")
                        .HasMaxLength(50);

                    b.Property<string>("RunWith")
                        .HasMaxLength(100);

                    b.Property<bool>("Sent");

                    b.Property<string>("SourceDepartment")
                        .HasMaxLength(100);

                    b.Property<string>("State")
                        .HasMaxLength(10);

                    b.Property<DateTime?>("SystemRefreshDate");

                    b.Property<DateTime?>("TerminationDate");

                    b.Property<string>("WorkInstructions");

                    b.HasKey("ID");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("ReportOverviewApp.Models.WidgetModels.Widget", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<string>("Footer");

                    b.Property<string>("Header");

                    b.Property<int>("SubWidgetID");

                    b.HasKey("ID");

                    b.ToTable("Widget");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ReportOverviewApp.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ReportOverviewApp.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
