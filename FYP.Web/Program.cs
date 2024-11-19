
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
using Microsoft.AspNetCore.Http.Features;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Set to Commercial if you have a license
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("default"));
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB
});
builder.Services.AddAuthorization();  // Add this line
builder.Services.AddAuthentication();  // Ensure authentication is also configured if needed
builder.Services.AddScoped<IStudentGroupRepository, StudentGroupRepository>();
builder.Services.AddScoped<IStudentGroupService, StudentGroupService>();
builder.Services.AddScoped<IWeeklyLogsRepository, WeeklyLogsRepository>();
builder.Services.AddScoped<IWeeklyLogsService, WeeklyLogsService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoomAllotmentRepository, RoomAllotmentRepository>();
builder.Services.AddScoped<IRoomAllotmentService, RoomAllotmentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<FYP.Repositories.Interfaces.IRoomInCharge, RoomInChargeRepository>();
builder.Services.AddScoped<FYP.Services.IRoomInChargeService, RoomInChargeService>();
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
builder.Services.AddScoped<IEvaluationCriteriaService, EvaluationCriteriaService>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IChangeTitleFormRepository, ChangeTitleFormRepository>();
builder.Services.AddScoped<IChangeTitleFormService, ChangeTitleFormService>();
builder.Services.AddScoped<IRoomRepository, RoomsRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IChangeSupervisorFormRepository, ChangeSupervisorFormRepository>();
builder.Services.AddScoped<IChangeSupervisorFormService, ChangeSupervisorFormService>();

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
