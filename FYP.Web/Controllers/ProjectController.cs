using FYP.Entities;
using FYP.Services;
using FYP.Services.Implementation;
using FYP.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace FYP.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IUserService _userService;
        private readonly Services.IRoomAllotmentService _supervisorService;
        private readonly IProposalDefenseService _proposalDefenseService;
        private readonly IRoomService _designationService;
        private readonly IStudentGroupService _studentGroupService;
        private readonly IProjectService _projectService;
        private readonly ICompanyService _companyService;
        private readonly IWeeklyLogsService _weeklyLogsService;
        private readonly IEvaluationService _evaluationService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEvaluationCriteriaService _evaluationCriteriaService;



        public ProjectController(IEvaluationCriteriaService evaluationCriteriaService, IWeeklyLogsService weeklyLogsService, IEvaluationService evaluationService, ICompanyService companyService, IProjectService projectService, IUserService userService, UserManager<AppUser> userManager, Services.IRoomAllotmentService supervisorService, IProposalDefenseService proposalDefense, IRoomService designationService, IStudentGroupService studentGroupService)
        {
            _userService = userService;
            _userManager = userManager;
            _proposalDefenseService = proposalDefense;
            _supervisorService = supervisorService;
            _studentGroupService = studentGroupService;
            _designationService = designationService;
            _projectService = projectService;
            _companyService = companyService;
            _evaluationService = evaluationService;
            _weeklyLogsService = weeklyLogsService;
            _evaluationCriteriaService = evaluationCriteriaService;

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Projects(string ViewValue)
        {
            if (ViewValue != null)
            {
                ViewBag.success = ViewValue;
            }
            var loggedInUser = _userManager.GetUserId(User);
            var allGroups = _studentGroupService.GetAllAsync().Result;
            var userGroup = allGroups
                .Where(x => x.student1LID == loggedInUser || x.student2ID == loggedInUser || x.student3ID == loggedInUser)
                .FirstOrDefault();

            if (userGroup == null)
            {
                return PartialView(new ProjectViewModel()); // Handle no group case
            }

            var projects = _projectService.GetAllAsync().Result
                .Where(x => x.groupId == userGroup.ID.ToString())
                .ToList();
            if (!projects.Any())
            {
                return RedirectToAction("Create", "Project");
            }
            var supervisorId = userGroup.SupervisorID;
            var model = new ProjectViewModel
            {
                projects = projects.Select(x => new ProjectViewModel
                {

                    code = x.code,
                    Specialization = x.Specialization,
                    Status = x.Status,
                    Summary = x.Summary,
                    commiteeId = x.commiteeId,
                    companyID = x.companyID,
                    ExpectedResults = x.ExpectedResults,
                    groupId = x.groupId,
                    objectives = x.objectives,
                    Others = x.Others,
                    Id = x.ID,
                    Title = x.Title,
                    Tools = x.Tools,
                    ProjectCategory = x.ProjectCategory,
                    projectGroup = x.projectGroup,
                    batch = x.batch,
                    groupname = userGroup.Name,
                    supervisorname = _userManager.FindByIdAsync(supervisorId).Result?.Name,
                    SupervsiorApproved = x.SupervsiorApproved,
                    ProId = x.ID.ToString(),
                }).ToList()
            };

            if (User.IsInRole("Student"))
            {
                supervisorId = userGroup.SupervisorID;
                var groupLeader = userGroup.student1LID;
                // Fetch all evaluation criteria from the service
                var fetchEvaluationCriteria = await _evaluationCriteriaService.GetAllAsync();

                if (fetchEvaluationCriteria != null)
                {
                    // Get the current date
                    var currentDate = DateTime.Now;

                    // Filter evaluations for the specific group (by GId) and check if one day has passed since submission
                    var groupEvaluations = fetchEvaluationCriteria
                        .Where(x => x.GId == userGroup.ID.ToString()
                                    && x.SubmissionDate.HasValue
                                    && (currentDate - x.SubmissionDate.Value).TotalDays > 1) // Ensure one day has passed
                        .ToList();

                }

                model.supervisorname = _userManager.FindByIdAsync(supervisorId).Result?.Name;
                model.groupname = userGroup.Name;
            }

            return PartialView(model);
        }
        public async Task<IActionResult> Create()
        {
            var project = await _projectService.GetAllAsync();
            var group = await _studentGroupService.GetAllAsync();
            var LoggedInUser = _userManager.GetUserId(User);

            var model = new ProjectViewModel()
            {
                projects = project.Select(x => new ProjectViewModel
                {
                    Specialization = x.Specialization,
                    Status = x.Status,
                    Summary = x.Summary,
                    commiteeId = x.commiteeId,
                    companyID = x.companyID,
                    ExpectedResults = x.ExpectedResults,
                    groupId = x.groupId,
                    objectives = x.objectives,
                    Others = x.Others,
                    Id = x.ID,
                    Title = x.Title,
                    Tools = x.Tools,
                    ProjectCategory = x.ProjectCategory,
                    projectGroup = x.projectGroup,
                }).ToList(),
            };

            if (User.IsInRole("Student"))
            {
                var getSupervisor = group
                    .Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser)
                    .FirstOrDefault().SupervisorID;

                var getGroup = group
                    .Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser)
                    .FirstOrDefault().ID;

                var getLeader = group
                    .Where(x => x.ID == getGroup)
                    .Select(x => x.student1LID)
                    .FirstOrDefault();

                model.supervisorname = (await _userManager.FindByIdAsync(getSupervisor)).Name;
                model.groupname = group
                    .Where(x => x.ID == getGroup)
                    .Select(x => x.Name)
                    .FirstOrDefault();
            }

            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var batch = "";

            // Determine batch based on the current month
            if (month > 2 && month < 6)
            {
                batch = "Spring" + (year + 1).ToString();
            }
            else
            {
                batch = "Fall" + year.ToString();
            }

            // Get the batch code (e.g., "SPR" or "FAL")
            var batchCode = batch.Substring(0, 3).ToUpper();

            // Fetch the last project code from the database that starts with "FYP + batchCode"
            var lastProjectCode = project
                .Where(p => p.code.StartsWith("FYP-" + batchCode))
                .OrderByDescending(p => p.code)
                .Select(p => p.code)
                .FirstOrDefault();

            int lastNumber = 0;

            if (!string.IsNullOrEmpty(lastProjectCode))
            {
                // Extract the numeric part after the batch prefix
                var numberPart = lastProjectCode.Substring(("FYP-" + batchCode + "-").Length);
                int.TryParse(numberPart, out lastNumber);
            }

            // Increment the last number by 1
            lastNumber += 1;

            // Format the new number as 5 digits
            var newNumber = lastNumber.ToString("D5");

            // Generate the new project code
            var newProjectCode = "FYP-" + batchCode + "-" + newNumber;

            model.code = newProjectCode;

            return PartialView(model);
        }
        public async Task<IActionResult> ProjectDetails(string code, string groupId,string Data)
        {
            // Fetch all necessary data asynchronously
            if (Data != null)
            {
                ViewBag.success = Data;
            }
            var studentGroups = await _studentGroupService.GetAllAsync();
            var projects = await _projectService.GetAllAsync();
            var companies = await _companyService.GetAllAsync();

            var project = projects.FirstOrDefault(x => x.code.ToString() == code);
            if (groupId !=null)
            {
                 project = projects.FirstOrDefault(x => x.groupId == groupId);
                if (project == null)
                {
                   
                        return RedirectToAction("StudentGroupAlongMarks", "Cordinator", new { ViewValue = "NotFound" });
                    
                }
            }
            if (project == null)
            {
                
                
                 return NotFound("Project not found");
                
            }

            // Find the student group associated with the project using groupId
            var userGroup = studentGroups.FirstOrDefault(x => x.ID.ToString() == project.groupId);
            if (userGroup == null)
            {
                return NotFound("Student group not found");
            }

            // Fetch the supervisor ID from the student group
            var supervisorId = userGroup.SupervisorID;

            // Retrieve the company details related to the project
            var company = companies.FirstOrDefault(x => x.ID.ToString() == project.companyID);

            // Prepare the ViewModel with all the necessary details
            var model = new ProjectViewModel
            {
                ProId = project.ID.ToString(),
                code = project.code,
                Specialization = project.Specialization,
                Status = project.Status,
                Summary = project.Summary,
                commiteeId = project.commiteeId,
                companyID = project.companyID,
                ExpectedResults = project.ExpectedResults,
                groupId = project.groupId,
                objectives = project.objectives,
                Others = project.Others,
                Id = project.ID,
                Title = project.Title,
                Tools = project.Tools,
                ProjectCategory = project.ProjectCategory,
                projectGroup = project.projectGroup,
                batch = project.batch,
                groupname = userGroup.Name,
                supervisorname = (await _userManager.FindByIdAsync(supervisorId))?.Name,
                companyName = company?.Name,
                companyMentor = company?.MentorName,
                mentorcontact = company?.MentorTelephone,
                mentoremail = company?.MentorEmail,
                SupervsiorApproved = project.SupervsiorApproved,
                changeTitleFormStatus = project.changeTitleFormStatus,
            };

            return PartialView(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditProjects(string id)
        {
            // Retrieve the project with the specified ID
            var fetchproject = await _projectService.GetAllAsync();
            var project = fetchproject.Where(x => x.ID.ToString() == id).FirstOrDefault();
            var Fetchcompany = await _companyService.GetAllAsync();
            var company = Fetchcompany.Where(x => x.ID.ToString() == project.companyID).FirstOrDefault();
            if (project == null)
            {
                return NotFound();
            }

            var group = await _studentGroupService.GetAllAsync();
            var LoggedInUser = _userManager.GetUserId(User);

            // Initialize the model with existing project data
            var model = new ProjectViewModel
            {
                Specialization = project.Specialization,
                Status = project.Status,
                Summary = project.Summary,
                commiteeId = project.commiteeId,
                companyID = project.companyID,
                ExpectedResults = project.ExpectedResults,
                groupId = project.groupId,
                objectives = project.objectives,
                Others = project.Others,
                Id = project.ID,
                ProId = project.ID.ToString(),
                Title = project.Title,
                Tools = project.Tools,
                ProjectCategory = project.ProjectCategory,
                projectGroup = project.projectGroup,
                code = project.code,
                batch = project.batch,
                changeTitleFormStatus = project.changeTitleFormStatus,
                companyMentor = company.MentorName,
                companyName = company.MentorName,
                mentorcontact = company.MentorTelephone,
                mentoremail = company.MentorEmail,

            };


            var studentGroup = group.Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser).FirstOrDefault();
            if (studentGroup != null)
            {
                var getSupervisor = studentGroup.SupervisorID;
                var getGroup = studentGroup.ID;
                var getLeader = studentGroup.student1LID;

                model.supervisorname = (await _userManager.FindByIdAsync(getSupervisor)).Name;
                model.groupname = group.Where(x => x.ID == getGroup).Select(x => x.Name).FirstOrDefault();
            }

            return PartialView(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditProjects([FromBody] ProjectViewModel model)
        {
            // Retrieve the existing project by ID
            var fetchProjects = await _projectService.GetAllAsync();
            var existingProject = fetchProjects.FirstOrDefault(x => x.ID.ToString() == model.ProId);

            if (existingProject == null)
            {
                return RedirectToAction("Projects", "Project", new { ViewValue = "NotFound" });
            }

            // Retrieve the company related to the project
            var fetchCompanies = await _companyService.GetAllAsync();
            var existingCompany = fetchCompanies.FirstOrDefault(x => x.ID.ToString() == model.companyID);

            if (existingCompany == null)
            {
                return RedirectToAction("Projects", "Project", new { ViewValue = "NotFound" });
            }

            // Update the project fields with the data from the model
            existingProject.Specialization = model.Specialization;
            existingProject.Status = null;
            existingProject.Summary = model.Summary;
            existingProject.ExpectedResults = model.ExpectedResults;
            existingProject.objectives = model.objectives;
            existingProject.Others = model.Others;
            existingProject.Title = model.Title;
            existingProject.Tools = model.Tools;
            existingProject.ProjectCategory = model.ProjectCategory;
            existingProject.projectGroup = model.projectGroup;
            existingProject.changeTitleFormStatus = model.changeTitleFormStatus;
            existingProject.SupervsiorApproved = null;

            // Update the company fields
            existingCompany.Name = model.companyName;
            existingCompany.MentorName = model.companyMentor;
            existingCompany.MentorTelephone = model.mentorcontact;
            existingCompany.MentorEmail = model.mentoremail;

            // Save the updated project
            await _projectService.UpdateAsync(existingProject);

            // Save the updated company (if needed)
            await _companyService.UpdateAsync(existingCompany);

            return RedirectToAction("Projects", "Project", new { ViewValue = "Updated" });
        }


        public async Task<IActionResult> ProjectList(string Id)
        {
            var LoggedInUser = _userManager.GetUserId(User);
            Project project = null;  // Declare project variable here

            if (Id == null)
            {
                var groupId = _studentGroupService.GetAllAsync().Result
                                  .Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser)
                                  .FirstOrDefault()?.ID;

                if (groupId != null)
                {
                    var projects = await _projectService.GetAllAsync();
                    project = projects.FirstOrDefault(x => x.groupId == groupId.ToString());
                }
            }
            else
            {
                var projects = await _projectService.GetAllAsync();
                project = projects.FirstOrDefault(x => x.ID.ToString() == Id);
            }

            if (project == null)
            {
                var models = new ProjectViewModel();
                models.CurrentProject = null;
                return PartialView(models);
            }

            var evaluations = await _evaluationService.GetAllAsync();
            var evaluationCriteria = await _evaluationCriteriaService.GetAllAsync();

            var evaluation = evaluations.FirstOrDefault(x => x.PBatch == project.batch && x.EvaluationName == "Proposal");
            var evaluationfinal = evaluations.FirstOrDefault(x => x.PBatch == project.batch && x.EvaluationName == "Final");
            var evaluationmid = evaluations.FirstOrDefault(x => x.PBatch == project.batch && x.EvaluationName == "Mid");

            var evaluationRemarks = evaluationCriteria.FirstOrDefault(x => x.Batch == project.batch && x.Batch == "Proposal");
            var evaluationfinalRemarks = evaluationCriteria.FirstOrDefault(x => x.Batch == project.batch && x.Batch == "Final");
            var evaluationmidRemarks = evaluationCriteria.FirstOrDefault(x => x.Batch == project.batch && x.Batch == "Mid");

            var evaluatioProposal = evaluationCriteria.FirstOrDefault(x => x.Batch == project.batch && x.EvalName == "Proposal")?.StudentsProposalMarks;

            // Fetch marks for each evaluation type, with null handling
            var finalMarks1 = evaluationCriteria.Where(x => x.Batch == project.batch && x.EvalName == "Final")
                                                .Select(x => x.Student1FinalMarks ?? 0)
                                                .ToList();

            var midMarks1 = evaluationCriteria.Where(x => x.Batch == project.batch && x.EvalName == "Mid")
                                              .Select(x => x.Student1MidMarks ?? 0)
                                              .ToList();

            int finalAverage1 = finalMarks1.Any() ? (int)Math.Round(finalMarks1.Average()) : 0;
            int midAverage1 = midMarks1.Any() ? (int)Math.Round(midMarks1.Average()) : 0;

            var finalMarks2 = evaluationCriteria.Where(x => x.Batch == project.batch && x.EvalName == "Final")
                                                .Select(x => x.Student2FinalMarks ?? 0)
                                                .ToList();

            var midMarks2 = evaluationCriteria.Where(x => x.Batch == project.batch && x.EvalName == "Mid")
                                              .Select(x => x.Student2MidMarks ?? 0)
                                              .ToList();

            int finalAverage2 = finalMarks2.Any() ? (int)Math.Round(finalMarks2.Average()) : 0;
            int midAverage2 = midMarks2.Any() ? (int)Math.Round(midMarks2.Average()) : 0;

            var finalMarks3 = evaluationCriteria.Where(x => x.Batch == project.batch && x.EvalName == "Final")
                                                .Select(x => x.Student3FinalMarks ?? 0)
                                                .ToList();

            var midMarks3 = evaluationCriteria.Where(x => x.Batch == project.batch && x.EvalName == "Mid")
                                              .Select(x => x.Student3MidMarks ?? 0)
                                              .ToList();

            int finalAverage3 = finalMarks3.Any() ? (int)Math.Round(finalMarks3.Average()) : 0;
            int midAverage3 = midMarks3.Any() ? (int)Math.Round(midMarks3.Average()) : 0;

            var model = new ProjectViewModel
            {
                code = project.code,
                Status = project.Status,
                CurrentProject = project,
                MidPPT = project.MidPPT,
                MidReport = project.MidReport,
                PropPPT = project.PropPPT,
                PropReport = project.PropReport,
                FinalDocs = project.FinalDocs,
                Proposal = evaluation?.EvaluationName ?? "N/A",
                Proposaldate = evaluation?.LastDate ?? DateTime.MinValue,
                Proposalbatch = evaluation?.PBatch ?? "N/A",
                final = evaluationfinal?.EvaluationName ?? "N/A",
                finaldate = evaluationfinal?.LastDate ?? DateTime.MinValue,
                finalbatch = evaluationfinal?.PBatch ?? "N/A",
                mid = evaluationmid?.EvaluationName ?? "N/A",
                middate = evaluationmid?.LastDate ?? DateTime.MinValue,
                midbatch = evaluationmid?.PBatch ?? "N/A",
                ExpectedResults = project.ExpectedResults,
                FinalRemark = evaluationfinalRemarks?.Remarks ?? "N/A",
                MidRemark = evaluationmidRemarks?.Remarks ?? "N/A",
                PropRemark = evaluationRemarks?.Remarks ?? "N/A",
                Proposalmarks = evaluatioProposal ?? 0
            };

            var fetchGroup = _studentGroupService.GetAllAsync().Result;
            var getGroup = fetchGroup.Where(x => x.ID.ToString() == project.groupId).FirstOrDefault();

            if (getGroup != null)
            {
                if (getGroup.student1LID == LoggedInUser)
                {
                    model.midmarks = midAverage1;
                    model.finalmarks = finalAverage1;
                }
                if (getGroup.student2ID == LoggedInUser)
                {
                    model.midmarks = midAverage2;
                    model.finalmarks = finalAverage2;
                }
                if (getGroup.student3ID == LoggedInUser)
                {
                    model.midmarks = midAverage3;
                    model.finalmarks = finalAverage3;
                }
            }

            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> Submissions([FromBody] ProjectViewModel model)
        {
            var project = await _projectService.GetAllAsync();
            var projectDetails = project.FirstOrDefault(x => x.code == model.code);

            if (projectDetails == null)
            {
                return NotFound();
            }

            bool isUpdated = false; // Track if any fields are updated

            if (!string.IsNullOrEmpty(model.PropPPT) && model.PropPPT != projectDetails.PropPPT)
            {
                projectDetails.PropPPT = model.PropPPT;
                projectDetails.ProposalSubmissionDate = DateTime.Now.ToString();
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(model.PropReport) && model.PropReport != projectDetails.PropReport)
            {
                projectDetails.PropReport = model.PropReport;
                projectDetails.ProposalSubmissionDate = DateTime.Now.ToString();
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(model.MidPPT) && model.MidPPT != projectDetails.MidPPT)
            {
                projectDetails.MidPPT = model.MidPPT;
                projectDetails.MidSubmissionDate = DateTime.Now.ToString();
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(model.MidReport) && model.MidReport != projectDetails.MidReport)
            {
                projectDetails.MidReport = model.MidReport;
                projectDetails.MidSubmissionDate = DateTime.Now.ToString();
                isUpdated = true;
            }

            if (!string.IsNullOrEmpty(model.FinalDocs) && model.FinalDocs != projectDetails.FinalDocs)
            {
                projectDetails.FinalDocs = model.FinalDocs;
                projectDetails.FinalSubmissionDate = DateTime.Now.ToString();
                isUpdated = true;
            }

            // Only update if something has changed
            if (isUpdated)
            {
                await _projectService.UpdateAsync(projectDetails);
            }

            return RedirectToAction("ProjectList", "Project");
        }

        public async Task<IActionResult> DeleteSubmissions([FromBody] ProjectViewModel model)
        {
            // Fetch all projects and filter by code
            var projects = await _projectService.GetAllAsync();
            var projectDetails = projects.FirstOrDefault(x => x.code == model.code);

            if (projectDetails == null)
            {
                return NotFound();
            }

            bool isDeleted = false;

            // Helper function to delete fields
            string TryDelete(string property, string doc)
            {
                if (property == doc)
                {
                    isDeleted = true;
                    return null; // Set the property to null if it matches
                }
                return property; // Return the original value if no match
            }

            // Check and potentially delete document fields
            projectDetails.PropPPT = TryDelete(projectDetails.PropPPT, model.Docs);
            projectDetails.PropReport = TryDelete(projectDetails.PropReport, model.Docs);
            projectDetails.MidPPT = TryDelete(projectDetails.MidPPT, model.Docs);
            projectDetails.MidReport = TryDelete(projectDetails.MidReport, model.Docs);
            projectDetails.FinalDocs = TryDelete(projectDetails.FinalDocs, model.Docs);

            // Only update if a field was deleted
            if (isDeleted)
            {
                await _projectService.UpdateAsync(projectDetails);
            }

            return RedirectToAction("ProjectList", "Project");
        }
        public IActionResult ChangeTitleForm(string code)
        {
            var projectDeatils = _projectService.GetAllAsync().Result.Where(x => x.code == code).FirstOrDefault();
            var group = _studentGroupService.GetAllAsync().Result;
            var LoggedInUser = _userManager.GetUserId(User);

            var project = new ProjectViewModel()
            {
                code = projectDeatils.code,
                batch = projectDeatils.batch,
                Title = projectDeatils.Title,
                groupId = group.ToString(),
                changeTitleFormStatus = projectDeatils.changeTitleFormStatus,
            };
            var getSupervisor = _studentGroupService.GetAllAsync().Result.Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser).FirstOrDefault().SupervisorID;
            var getGroup = _studentGroupService.GetAllAsync().Result.Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser).FirstOrDefault().ID;
            var getLeader = _studentGroupService.GetAllAsync().Result.Where(x => x.ID == getGroup).Select(x => x.student1LID).FirstOrDefault();
            project.supervisorname = _userManager.FindByIdAsync(getSupervisor).Result.Name;
            project.groupname = _studentGroupService.GetAllAsync().Result.Where(x => x.ID == getGroup).Select(x => x.Name).FirstOrDefault();
            return PartialView(project);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeSupervisorApprovedStatus(string Id, string status,string Data)
        {
            // Fetch the project by ID
            var project = (await _projectService.GetAllAsync()).FirstOrDefault(x => x.ID.ToString() == Id);

            if (project == null)
            {
                // Handle the case where the project is not found
                return NotFound();
            }

            // Change the status of the project
            project.SupervsiorApproved = status;

            // Update the project asynchronously
            var result = _projectService.UpdateAsync(project);

            if (result.IsCompletedSuccessfully)
            {
                if (Data == "NG")
                {
                    return RedirectToAction("Projects", "Supervisor");
                }
                else
                {
                    return RedirectToAction("StudentGroupAlongMarks", "cordinator");

                }
            }

                // In case the update fails, handle it accordingly (e.g., show an error message)
                return BadRequest("Failed to update the project status.");
        }

        public async Task<IActionResult> weeklylog()
        {
            var group = await _studentGroupService.GetAllAsync();
            var LoggedInUser = _userManager.GetUserId(User);
            var groupId = group.Where(x => x.student1LID == LoggedInUser || x.student2ID == LoggedInUser || x.student3ID == LoggedInUser).FirstOrDefault().ID;
            var groupName = group.Where(x => x.ID == groupId).FirstOrDefault().Name;
            var getProject = await _projectService.GetAllAsync();
            var getgroupProject = getProject.Where(x => x.groupId == groupId.ToString()).FirstOrDefault();
            if (getgroupProject == null)
            {
                var models = new WeeklyLogsViewModel();
                models.CurrentProject = null;
                return PartialView(models);

            }
            var getBatch = getProject.Where(x => x.groupId == groupId.ToString()).FirstOrDefault().batch;
            var getStatus = getProject.Where(x => x.groupId == groupId.ToString()).FirstOrDefault().Status;
            var evaluation = await _evaluationService.GetAllAsync();
            var getProposalDate = evaluation.Where(x => x.PBatch == getBatch && x.EvaluationName == "Proposal").FirstOrDefault().LastDate;
            if (getProposalDate == null)
            {
                var models = new WeeklyLogsViewModel();
                models.CurrentPropDate = null;
                return PartialView(models);

            }
            var existingLogs = await _weeklyLogsService.GetAllAsync();
            var groupLogs = existingLogs.Where(x => x.GroupId == groupId.ToString());

            if (groupLogs == null || !groupLogs.Any())
            {
                // Get the proposal date
                var proposalDate = evaluation
                    .Where(x => x.PBatch == getBatch && x.EvaluationName == "Proposal")
                    .FirstOrDefault()?.LastDate;

                if (proposalDate != null)
                {
                    proposalDate = proposalDate.Value.AddDays(-7);

                    // Assign 12 weekly logs
                    for (int i = 1; i <= 16; i++)
                    {
                        var weeklyLog = new WeeklyLogs
                        {

                            GroupId = groupId.ToString(),
                            WeekNo = $"Week {i}",
                            AssignDate = proposalDate.Value.AddDays(i * 7), // Add 7 days per week
                            SubmitDate = proposalDate.Value.AddDays(i * 7 + 7), // Submission date one week after assignment
                            RoomNo = string.Empty, // Set room no if needed
                            Summary = string.Empty, // Initial summary is empty
                            Activities = string.Empty, // Initial activities are empty
                            Result = string.Empty, // Initial result is empty
                            Tasks = string.Empty, // Initial tasks are empty
                            Status = "Pending"
                        };
                        await _weeklyLogsService.AddAsync(weeklyLog); // Assuming AddAsync is your method to save the log
                    }
                }
            }
            var midDate = evaluation
                       .Where(x => x.PBatch == getBatch && x.EvaluationName == "Mid")
                       .FirstOrDefault()?.LastDate;
            if (midDate == null)
            {
                var models = new WeeklyLogsViewModel();
                models.CurrentMidDate = null;
                return PartialView(models);

            }
            var getWeeks = groupLogs.Select(x => x.WeekNo).LastOrDefault();
            if (getWeeks != "Week 32")
            {
                if (midDate != null)
                {
                    // Assign 12 weekly logs
                    for (int i = 17; i <= 32; i++)
                    {
                        var weeklyLog = new WeeklyLogs
                        {
                            GroupId = groupId.ToString(),
                            WeekNo = $"Week {i}",
                            AssignDate = midDate.Value.AddDays(i * 7), // Add 7 days per week
                            SubmitDate = midDate.Value.AddDays(i * 7 + 7), // Submission date one week after assignment
                            RoomNo = string.Empty, // Set room no if needed
                            Summary = string.Empty, // Initial summary is empty
                            Activities = string.Empty, // Initial activities are empty
                            Result = string.Empty, // Initial result is empty
                            Tasks = string.Empty, // Initial tasks are empty
                            Status = "Pending"
                        };
                        await _weeklyLogsService.AddAsync(weeklyLog); // Assuming AddAsync is your method to save the log
                    }
                }
            }
            var viewModel = new WeeklyLogsViewModel();
            var currentDate = DateTime.Now;

            var getCurrentWeek = groupLogs.Where(x => x.SubmitDate >= currentDate).FirstOrDefault();
            if (getCurrentWeek != null)
            {
                viewModel.WeekNo = getCurrentWeek.WeekNo;
                viewModel.AssignDate = getCurrentWeek.AssignDate;
                viewModel.SubmitDate = getCurrentWeek.SubmitDate;
                viewModel.RoomNo = getCurrentWeek.RoomNo;
                viewModel.Summary = getCurrentWeek.Summary;
                viewModel.Activities = getCurrentWeek.Activities;
                viewModel.Result = getCurrentWeek.Result;
                viewModel.Tasks = getCurrentWeek.Tasks;
                viewModel.Status = getCurrentWeek.Status;
                viewModel.UserSubmissionDate = getCurrentWeek.UserSubmissionDate;
                viewModel.GroupId = getCurrentWeek.GroupId;
                viewModel.currentDate = currentDate;
                viewModel.ProjectStatus = getStatus;
            }

            viewModel.weeklies = groupLogs.Select(x => new WeeklyLogsViewModel
            {
                ProjectStatus = _projectService.GetAllAsync().Result.Where(x => x.groupId == groupId.ToString()).FirstOrDefault().Status,
                WeekNo = x.WeekNo,
                GroupName = groupName,
                RoomNo = x.RoomNo,
                Summary = x.Summary,
                Activities = x.Activities,
                Result = x.Result,
                Tasks = x.Tasks,
                AssignDate = x.AssignDate,
                SubmitDate = x.SubmitDate,
                currentDate = DateTime.Now
            }).ToList();

            viewModel.CurrentPropDate = getProposalDate.ToString();
            viewModel.CurrentMidDate = midDate.ToString();
            viewModel.CurrentProject = getBatch;
            return PartialView(viewModel);
        }
        public async Task<IActionResult> getWeekLog(string weekLog, string groupId)
        {
            var Week = await _weeklyLogsService.GetAllAsync();
            var getWeek = Week.Where(x => x.WeekNo == weekLog && x.GroupId == groupId).FirstOrDefault();
            var getProject = await _projectService.GetAllAsync();
            var getBatch = getProject.Where(x => x.groupId == groupId.ToString()).FirstOrDefault()?.batch;
            var getStatus = getProject.Where(x => x.groupId == groupId.ToString()).FirstOrDefault().Status;
            var existingLogs = await _weeklyLogsService.GetAllAsync();
            var groupLogs = existingLogs.Where(x => x.GroupId == groupId.ToString());
            var group = await _studentGroupService.GetAllAsync();
            var groupName = group.Where(x => x.ID.ToString() == groupId).FirstOrDefault().Name;
            if (getBatch == null)
            {
                return Json(new { success = false, message = "Batch not found" });
            }

            var viewModel = new WeeklyLogsViewModel
            {
                ProjectStatus = getStatus,
                CurrentProject = getBatch,
                WeekNo = getWeek.WeekNo,
                RoomNo = getWeek.RoomNo,
                Summary = getWeek.Summary,
                Activities = getWeek.Activities,
                Result = getWeek.Result,
                Tasks = getWeek.Tasks,
                AssignDate = getWeek.AssignDate,
                SubmitDate = getWeek.SubmitDate,
                Status = getWeek.Status,
                UserSubmissionDate = getWeek.UserSubmissionDate,
                GroupId = getWeek.GroupId,
            };
            viewModel.weeklies = groupLogs.Select(x => new WeeklyLogsViewModel
            {

                WeekNo = x.WeekNo,
                GroupName = groupName,
                RoomNo = x.RoomNo,
                Summary = x.Summary,
                Activities = x.Activities,
                Result = x.Result,
                Tasks = x.Tasks,
                AssignDate = x.AssignDate,
                SubmitDate = x.SubmitDate,
                currentDate = DateTime.Now
            }).ToList();
            return PartialView("weeklylog", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateWeeklyLog([FromBody] WeeklyLogsViewModel model)
        {
            // Fetch existing log
            var weekLog = await _weeklyLogsService.GetAllAsync();
            var getGroupWeeklyLog = weekLog
                .Where(x => x.WeekNo == model.WeekNo && x.GroupId == model.GroupId)
                .FirstOrDefault();

            if (getGroupWeeklyLog != null)
            {
                // Compare each field and update only if different from the existing value

                if (getGroupWeeklyLog.RoomNo != model.RoomNo)
                {
                    getGroupWeeklyLog.RoomNo = model.RoomNo;
                }

                if (getGroupWeeklyLog.Summary != model.Summary)
                {
                    getGroupWeeklyLog.Summary = model.Summary;
                }

                if (getGroupWeeklyLog.Activities != model.Activities)
                {
                    getGroupWeeklyLog.Activities = model.Activities;
                }

                if (getGroupWeeklyLog.Result != model.Result)
                {
                    getGroupWeeklyLog.Result = model.Result;
                }

                if (getGroupWeeklyLog.Tasks != model.Tasks)
                {
                    getGroupWeeklyLog.Tasks = model.Tasks;
                }

                if (getGroupWeeklyLog.Status != model.Status)
                {
                    getGroupWeeklyLog.Status = model.Status;
                }

                // Always update the submission date
                getGroupWeeklyLog.UserSubmissionDate = DateTime.Now;

                // Update the database
                await _weeklyLogsService.UpdateAsync(getGroupWeeklyLog);

                // Redirect to getWeekLog after update
                return RedirectToAction("getWeekLog", "Project", new { weekLog = model.WeekNo, groupId = model.GroupId });
            }

            // If the log was not found
            return Json(new { success = false, message = "Weekly log not found" });
        }

    }
}
