
using FYP.Db;
using FYP.Entities;
using FYP.Repositories.Implementation;
using FYP.Repositories.Interfaces;
using FYP.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FYP.Services;
using FYP.Services.Implementation;
using FYP.Databases;
using FYP.Web.ChatHub;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("default"));
});
builder.Services.AddAuthorization();  // Add this line
builder.Services.AddAuthentication();  // Ensure authentication is also configured if needed
builder.Services.AddScoped<IStudentGroupRepository, StudentGroupRepository>();
builder.Services.AddScoped<IStudentGroupService, StudentGroupService>();
builder.Services.AddScoped<IWeeklyLogsRepository, WeeklyLogsRepository>();
builder.Services.AddScoped<IWeeklyLogsService, WeeklyLogsService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISupervisorRepository, SupervisorRepository>();
builder.Services.AddScoped<ISupervisorService, SupervisorService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISemesterManagementRepository, SemesterManagementRepository>();
builder.Services.AddScoped<ISemesterManagementService, SemesterManagementService>();
builder.Services.AddScoped<IProposalDefenseRepository, ProposalDefenseRepository>();
builder.Services.AddScoped<IProposalDefenseService, ProposalDefenseService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectCategoryRepository, ProjectCategoryRepository>();
builder.Services.AddScoped<IProjectCategoryService, ProjectCategoryService>();
builder.Services.AddScoped<IFYPCommitteRepository, FYPCommitteRepository>();
builder.Services.AddScoped<IFYPCommitteService, FYPCommitteService>();
builder.Services.AddScoped<IEvaluationRepository, EvaluationRepository>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddScoped<IEvaluationCriteriaRepository, EvaluationCriteriaRepository>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IChangeTitleFormRepository, ChangeTitleFormRepository>();
builder.Services.AddScoped<IChangeTitleFormService, ChangeTitleFormService>();
builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<IDesignationService, DesignationService>();

builder.Services.AddIdentity<AppUser, IdentityRole>(
    options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    await DbSeeder.SeedRolesAndAdminAsync(scope.ServiceProvider);
}
app.Run();
