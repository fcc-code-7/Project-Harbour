using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FYP.Entities;
using System.Collections.Generic;
using FYP.Models;

namespace FYP.Db
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ChangeTitleForm> ChangeTitleForms { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<EvaluationCriteria> EvaluationCriterias { get; set; }
        public DbSet<FYPCommitte> FYPCommittes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectCategory> ProjectCategories { get; set; }
        public DbSet<ProposalDefense> ProposalDefenses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<WeeklyLogs> WeeklyLogs { get; set; }
        public DbSet<SemesterManagement> SemesterManagements { get; set; }
        public DbSet<ChangeTitleForm> changeTitleForms { get; set; }
        public DbSet<ChangeSupervisorForm> changeSupervisorForms { get; set; }
    }
}
