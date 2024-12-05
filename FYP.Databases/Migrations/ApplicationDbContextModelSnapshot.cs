﻿// <auto-generated />
using System;
using FYP.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("FYP.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActiveState")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnic")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("DateofCreation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Department")
                        .HasColumnType("TEXT");

                    b.Property<string>("Designation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Docs")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telephone")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("areaofintrest")
                        .HasColumnType("TEXT");

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
                        .HasColumnType("TEXT");

                    b.Property<string>("CommiteeID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<bool>("SupervisorStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("oldPID")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("ChangeTitleForm");
                });

            modelBuilder.Entity("FYP.Entities.Company", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("MentorEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("MentorName")
                        .HasColumnType("TEXT");

                    b.Property<string>("MentorTelephone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("FYP.Entities.Evaluation", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("EvaluationName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Marks")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PBatch")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Evaluations");
                });

            modelBuilder.Entity("FYP.Entities.EvaluationCriteria", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Batch")
                        .HasColumnType("TEXT");

                    b.Property<string>("CommiteeID")
                        .HasColumnType("TEXT");

                    b.Property<string>("EvalName")
                        .HasColumnType("TEXT");

                    b.Property<string>("GId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q1Marks")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q2Marks")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q3Marks")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q4Marks")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q5Marks")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q6Marks")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q7Marks")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q8Marks")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("SubmissionDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("SubmissionTime")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalMarks")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("EvaluationCriterias");
                });

            modelBuilder.Entity("FYP.Entities.FYPCommitte", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("AppointedDate")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("AppointedTime")
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("Endime")
                        .HasColumnType("TEXT");

                    b.Property<string>("EvaluationID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Member1ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Member2ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Room")
                        .HasColumnType("TEXT");

                    b.Property<string>("batch")
                        .HasColumnType("TEXT");

                    b.Property<string>("groupID")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("FYPCommittes");
                });

            modelBuilder.Entity("FYP.Entities.Project", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExpectedResults")
                        .HasColumnType("TEXT");

                    b.Property<string>("FinalDocs")
                        .HasColumnType("TEXT");

                    b.Property<string>("MidPPT")
                        .HasColumnType("TEXT");

                    b.Property<string>("MidReport")
                        .HasColumnType("TEXT");

                    b.Property<string>("Others")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProjectCategory")
                        .HasColumnType("TEXT");

                    b.Property<string>("PropPPT")
                        .HasColumnType("TEXT");

                    b.Property<string>("PropReport")
                        .HasColumnType("TEXT");

                    b.Property<string>("Specialization")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT");

                    b.Property<string>("SupervsiorApproved")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tools")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalMarks")
                        .HasColumnType("INTEGER");

                    b.Property<string>("batch")
                        .HasColumnType("TEXT");

                    b.Property<string>("changeTitleFormStatus")
                        .HasColumnType("TEXT");

                    b.Property<string>("code")
                        .HasColumnType("TEXT");

                    b.Property<string>("commiteeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("companyID")
                        .HasColumnType("TEXT");

                    b.Property<string>("groupId")
                        .HasColumnType("TEXT");

                    b.Property<string>("objectives")
                        .HasColumnType("TEXT");

                    b.Property<string>("projectGroup")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("FYP.Entities.ProjectCategory", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CordinatorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("ProjectCategories");
                });

            modelBuilder.Entity("FYP.Entities.ProposalDefense", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CommiteeID")
                        .HasColumnType("TEXT");

                    b.Property<int>("Marks")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PropID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q1")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q10")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q2")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q3")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q4")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q5")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q6")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q7")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q8")
                        .HasColumnType("TEXT");

                    b.Property<string>("Q9")
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("ProposalDefenses");
                });

            modelBuilder.Entity("FYP.Entities.RoomAllotment", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Batch")
                        .HasColumnType("TEXT");

                    b.Property<string>("Evaluation")
                        .HasColumnType("TEXT");

                    b.Property<string>("InchargeId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Room")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("RoomAllotments");
                });

            modelBuilder.Entity("FYP.Entities.Student", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ENo")
                        .HasColumnType("TEXT");

                    b.Property<int>("RegNo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Semester")
                        .HasColumnType("TEXT");

                    b.Property<string>("studentId")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("FYP.Entities.StudentGroup", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Batch")
                        .HasColumnType("TEXT");

                    b.Property<string>("CoSupervisorID")
                        .HasColumnType("TEXT");

                    b.Property<string>("CordinatorID")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("SupervisorID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Year")
                        .HasColumnType("TEXT");

                    b.Property<bool>("changeSupervisorForm")
                        .HasColumnType("INTEGER");

                    b.Property<string>("companyID")
                        .HasColumnType("TEXT");

                    b.Property<string>("student1LID")
                        .HasColumnType("TEXT");

                    b.Property<string>("student2ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("student3ID")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("StudentGroups");
                });

            modelBuilder.Entity("FYP.Entities.WeeklyLogs", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Activities")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("AssignDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Result")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomNo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SubmitDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tasks")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UserSubmissionDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("WeekNo")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("WeeklyLogs");
                });

            modelBuilder.Entity("FYP.Models.ChangeSupervisorForm", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CurrentDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupId")
                        .HasColumnType("TEXT");

                    b.Property<string>("NewSupervsiorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OtherReason")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reason")
                        .HasColumnType("TEXT");

                    b.Property<string>("oldsupervisorId")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("changeSupervisorForms");
                });

            modelBuilder.Entity("FYP.Models.Notifications", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Batch")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .HasColumnType("TEXT");

                    b.Property<string>("SenderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("FYP.Models.Room", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Department")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomNo")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("FYP.Models.RoomInCharge", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("RoomInCharge");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

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
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

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
