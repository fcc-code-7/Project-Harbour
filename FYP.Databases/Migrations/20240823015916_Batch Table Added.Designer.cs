﻿// <auto-generated />
using System;
using FYP.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FYP.Databases.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240823015916_Batch Table Added")]
    partial class BatchTableAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("FYP.Entities.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cnic")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Department")
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
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("LeaderStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NewPName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("SupervisorStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("oldPID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("ChangeTitleForms");
                });

            modelBuilder.Entity("FYP.Entities.Company", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("FYP.Entities.Evaluation", b =>
                {
                    b.Property<string>("EvaluationID")
                        .HasColumnType("TEXT");

                    b.Property<string>("CommiteeID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Marks")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PDefId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EvaluationID");

                    b.ToTable("Evaluations");
                });

            modelBuilder.Entity("FYP.Entities.EvaluationCriteria", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("EvalName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q1Desc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q1Marks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q2")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q2Desc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q2Marks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q3")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q3Desc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q3Marks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q4")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q4Desc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q4Marks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q5")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q5Desc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q5Marks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalMarks")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("EvaluationCriterias");
                });

            modelBuilder.Entity("FYP.Entities.FYPCommitte", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Member1ID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Member2ID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("groupID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("FYPCommittes");
                });

            modelBuilder.Entity("FYP.Entities.Project", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("PropDefID")
                        .IsRequired()
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
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("ProjectCategories");
                });

            modelBuilder.Entity("FYP.Entities.Proposal", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExpectedResults")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Others")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProjectCategory")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tools")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("commiteeId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("companyID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("groupId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("objectives")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("projectGroup")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Proposals");
                });

            modelBuilder.Entity("FYP.Entities.ProposalDefense", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("CommiteeID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Marks")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PropID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q10")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q2")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q3")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q4")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q5")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q6")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q7")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q8")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Q9")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("ProposalDefenses");
                });

            modelBuilder.Entity("FYP.Entities.Student", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ENo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RegNo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Semester")
                        .IsRequired()
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

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("SupervisorID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Year")
                        .HasColumnType("TEXT");

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

            modelBuilder.Entity("FYP.Entities.Supervisor", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Supervisors");
                });

            modelBuilder.Entity("FYP.Entities.WeeklyLogs", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Activities")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomNo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tasks")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("WeeklyLogs");
                });

            modelBuilder.Entity("FYP.Models.SemesterManagement", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Batch")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("SemesterManagements");
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
