﻿// <auto-generated />
using System;
using FYP.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FYP.Databases.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("FYP.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ActiveState")
                        .HasColumnType("longtext");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("Cnic")
                        .HasColumnType("longtext");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<DateOnly>("DateofCreation")
                        .HasColumnType("date");

                    b.Property<string>("Department")
                        .HasColumnType("longtext");

                    b.Property<string>("Designation")
                        .HasColumnType("longtext");

                    b.Property<string>("Docs")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Role")
                        .HasColumnType("longtext");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Telephone")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("areaofintrest")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("FYP.Entities.ChangeTitleForm", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CommiteeID")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<bool>("SupervisorStatus")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("oldPID")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("ChangeTitleForm");
                });

            modelBuilder.Entity("FYP.Entities.Company", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("MentorEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("MentorName")
                        .HasColumnType("longtext");

                    b.Property<string>("MentorTelephone")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("FYP.Entities.Evaluation", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("EvaluationName")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("LastDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Marks")
                        .HasColumnType("int");

                    b.Property<string>("PBatch")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Evaluations");
                });

            modelBuilder.Entity("FYP.Entities.EvaluationCriteria", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Batch")
                        .HasColumnType("longtext");

                    b.Property<string>("CommiteeID")
                        .HasColumnType("longtext");

                    b.Property<int?>("CordinatorMarks")
                        .HasColumnType("int");

                    b.Property<string>("EvalName")
                        .HasColumnType("longtext");

                    b.Property<string>("GId")
                        .HasColumnType("longtext");

                    b.Property<string>("Q1")
                        .HasColumnType("longtext");

                    b.Property<string>("Q2")
                        .HasColumnType("longtext");

                    b.Property<string>("Q3")
                        .HasColumnType("longtext");

                    b.Property<string>("Q4")
                        .HasColumnType("longtext");

                    b.Property<string>("Q5")
                        .HasColumnType("longtext");

                    b.Property<string>("Q6")
                        .HasColumnType("longtext");

                    b.Property<string>("Q7")
                        .HasColumnType("longtext");

                    b.Property<string>("Q8")
                        .HasColumnType("longtext");

                    b.Property<string>("Remarks")
                        .HasColumnType("longtext");

                    b.Property<int?>("Student1FinalMarks")
                        .HasColumnType("int");

                    b.Property<int?>("Student1MidMarks")
                        .HasColumnType("int");

                    b.Property<int?>("Student2FinalMarks")
                        .HasColumnType("int");

                    b.Property<int?>("Student2MidMarks")
                        .HasColumnType("int");

                    b.Property<int?>("Student3FinalMarks")
                        .HasColumnType("int");

                    b.Property<int?>("Student3MidMarks")
                        .HasColumnType("int");

                    b.Property<int?>("StudentsProposalMarks")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SubmissionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("SubmissionTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("SupervisorMarks")
                        .HasColumnType("int");

                    b.Property<int>("TotalMarks")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("EvaluationCriterias");
                });

            modelBuilder.Entity("FYP.Entities.FYPCommitte", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("AppointedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeOnly>("AppointedTime")
                        .HasColumnType("time(6)");

                    b.Property<TimeOnly>("Endime")
                        .HasColumnType("time(6)");

                    b.Property<string>("EvaluationID")
                        .HasColumnType("longtext");

                    b.Property<string>("ExternalId")
                        .HasColumnType("longtext");

                    b.Property<string>("Member1ID")
                        .HasColumnType("longtext");

                    b.Property<string>("Member2ID")
                        .HasColumnType("longtext");

                    b.Property<string>("Room")
                        .HasColumnType("longtext");

                    b.Property<string>("batch")
                        .HasColumnType("longtext");

                    b.Property<string>("groupID")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("FYPCommittes");
                });

            modelBuilder.Entity("FYP.Entities.Project", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ExpectedResults")
                        .HasColumnType("longtext");

                    b.Property<string>("FinalDocs")
                        .HasColumnType("longtext");

                    b.Property<string>("FinalSubmissionDate")
                        .HasColumnType("longtext");

                    b.Property<string>("MidPPT")
                        .HasColumnType("longtext");

                    b.Property<string>("MidReport")
                        .HasColumnType("longtext");

                    b.Property<string>("MidSubmissionDate")
                        .HasColumnType("longtext");

                    b.Property<string>("Others")
                        .HasColumnType("longtext");

                    b.Property<string>("ProjectCategory")
                        .HasColumnType("longtext");

                    b.Property<string>("PropPPT")
                        .HasColumnType("longtext");

                    b.Property<string>("PropReport")
                        .HasColumnType("longtext");

                    b.Property<string>("ProposalSubmissionDate")
                        .HasColumnType("longtext");

                    b.Property<string>("Specialization")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.Property<string>("SupervsiorApproved")
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .HasColumnType("longtext");

                    b.Property<string>("Tools")
                        .HasColumnType("longtext");

                    b.Property<int>("TotalMarks")
                        .HasColumnType("int");

                    b.Property<string>("batch")
                        .HasColumnType("longtext");

                    b.Property<string>("changeTitleFormStatus")
                        .HasColumnType("longtext");

                    b.Property<string>("code")
                        .HasColumnType("longtext");

                    b.Property<string>("commiteeId")
                        .HasColumnType("longtext");

                    b.Property<string>("companyID")
                        .HasColumnType("longtext");

                    b.Property<string>("groupId")
                        .HasColumnType("longtext");

                    b.Property<string>("objectives")
                        .HasColumnType("longtext");

                    b.Property<string>("projectGroup")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("FYP.Entities.ProjectCategory", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CordinatorId")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("ProjectCategories");
                });

            modelBuilder.Entity("FYP.Entities.ProposalDefense", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CommiteeID")
                        .HasColumnType("longtext");

                    b.Property<int>("Marks")
                        .HasColumnType("int");

                    b.Property<string>("PropID")
                        .HasColumnType("longtext");

                    b.Property<string>("Q1")
                        .HasColumnType("longtext");

                    b.Property<string>("Q10")
                        .HasColumnType("longtext");

                    b.Property<string>("Q2")
                        .HasColumnType("longtext");

                    b.Property<string>("Q3")
                        .HasColumnType("longtext");

                    b.Property<string>("Q4")
                        .HasColumnType("longtext");

                    b.Property<string>("Q5")
                        .HasColumnType("longtext");

                    b.Property<string>("Q6")
                        .HasColumnType("longtext");

                    b.Property<string>("Q7")
                        .HasColumnType("longtext");

                    b.Property<string>("Q8")
                        .HasColumnType("longtext");

                    b.Property<string>("Q9")
                        .HasColumnType("longtext");

                    b.Property<string>("Remarks")
                        .HasColumnType("longtext");

                    b.Property<bool>("Status")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("ID");

                    b.ToTable("ProposalDefenses");
                });

            modelBuilder.Entity("FYP.Entities.RoomAllotment", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Batch")
                        .HasColumnType("longtext");

                    b.Property<string>("Evaluation")
                        .HasColumnType("longtext");

                    b.Property<string>("InchargeId")
                        .HasColumnType("longtext");

                    b.Property<string>("Room")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("RoomAllotments");
                });

            modelBuilder.Entity("FYP.Entities.Student", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ENo")
                        .HasColumnType("longtext");

                    b.Property<int>("RegNo")
                        .HasColumnType("int");

                    b.Property<string>("Semester")
                        .HasColumnType("longtext");

                    b.Property<string>("studentId")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("FYP.Entities.StudentGroup", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Batch")
                        .HasColumnType("longtext");

                    b.Property<string>("CoSupervisorID")
                        .HasColumnType("longtext");

                    b.Property<string>("CordinatorID")
                        .HasColumnType("longtext");

                    b.Property<string>("ExternalId")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("SupervisorID")
                        .HasColumnType("longtext");

                    b.Property<string>("Year")
                        .HasColumnType("longtext");

                    b.Property<bool>("changeSupervisorForm")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("companyID")
                        .HasColumnType("longtext");

                    b.Property<string>("student1LID")
                        .HasColumnType("longtext");

                    b.Property<string>("student2ID")
                        .HasColumnType("longtext");

                    b.Property<string>("student3ID")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("StudentGroups");
                });

            modelBuilder.Entity("FYP.Entities.WeeklyLogs", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Activities")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("AssignDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GroupId")
                        .HasColumnType("longtext");

                    b.Property<string>("Result")
                        .HasColumnType("longtext");

                    b.Property<string>("RoomNo")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("SubmitDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext");

                    b.Property<string>("Tasks")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UserSubmissionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("WeekNo")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("WeeklyLogs");
                });

            modelBuilder.Entity("FYP.Models.ChangeSupervisorForm", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CurrentDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GroupId")
                        .HasColumnType("longtext");

                    b.Property<string>("NewSupervsiorId")
                        .HasColumnType("longtext");

                    b.Property<string>("OtherReason")
                        .HasColumnType("longtext");

                    b.Property<string>("Reason")
                        .HasColumnType("longtext");

                    b.Property<string>("oldsupervisorId")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("changeSupervisorForms");
                });

            modelBuilder.Entity("FYP.Models.Notifications", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Batch")
                        .HasColumnType("longtext");

                    b.Property<string>("Date")
                        .HasColumnType("longtext");

                    b.Property<string>("Message")
                        .HasColumnType("longtext");

                    b.Property<string>("SenderId")
                        .HasColumnType("longtext");

                    b.Property<string>("Subject")
                        .HasColumnType("longtext");

                    b.Property<string>("UserType")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("FYP.Models.Room", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Department")
                        .HasColumnType("longtext");

                    b.Property<string>("RoomNo")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("FYP.Models.RoomInCharge", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("RoomInCharge");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FYP.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FYP.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FYP.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FYP.Entities.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
