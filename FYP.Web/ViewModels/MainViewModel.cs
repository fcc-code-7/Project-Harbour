using FYP.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FYP.Web.ViewModels
{
    public class MainViewModel
    {
        public StudentGroupViewModel StudentGroup { get; set; }
    }
    public class StudentGroupViewModel 
    {
        public string? GrouID { get; set; }
        public Guid Id;
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [MaxLength(50) , MinLength(5)]
        public string Name { get; set; }
        public string student1LID { get; set; }
        //[DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string student1LEmail { get; set; }
        public string student2ID { get; set; }
        //[DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string student2Email { get; set; }
        public string student3ID { get; set; }
        //[DataType(DataType.EmailAddress)]
        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string student3Email { get; set; }
        public string Batch { get; set; }
        public string Year { get; set; }
        public string groupId { get; set; }
        public string SupervisorID { get; set; }
        public string CoSupervisorID { get; set; }
        public string companyID { get; set; }
        public string CordinatorID { get; set; }
        public string projectname { get; set; }
        public string LeaderName { get; set; }
        public string member1 { get; set; }
        public string Member2 { get; set; }
        public string supervisorname { get; set; }

        public List<string> Batches { get; set; }
        public List<string> Years { get; set; }
        public List<string> Supervisors { get; set; }
        public bool changeSupervisorForm { get; set; }
        public string? Member1Name { get; set; }
        public string? Member2Name { get; set; }
        public IEnumerable<StudentGroupViewModel> StudentGroups { get; set; }


    }
    public class ChangeSupervisorFormViewModel
    {
        public  Guid Id { get; set; }
        public string? NewSupervsiorId { get; set; }
        public string? oldsupervisorId { get; set; }
        public string? oldsupervsiorname { get; set; }
        public string? newsupervsiorname { get; set; }
        public string? OtherReason { get; set; }


        public string? Reason { get; set; }
        public string? GroupId { get; set; }
        public string? groupName { get; set; }
        public DateTime CurrentDate { get; set; }

        public List<SupervisorsViewModel> Supervisors { get; set; }

    }

    public class RoomInChargeViewModel
    {
        public string? Id { get; set; }

        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? RoomAlloted { get; set; }
        public string? Batch { get; set; }
        public string? Evaluation { get; set; }
        public List<RoomInChargeViewModel> roomInCharges { get; set; }
        public List<RoomInChargeViewModel> AllotedRoomIncharges { get; set; }
        public List<string> Incharges { get; set; }
        public List<string> Batchs { get; set; }
        public List<RoomViewModel> Rooms { get; set; }


    }
    public class FYPCommitteViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Room Incharge Email is required")]
        public string? RoomInChargeEmail { get; set; }
        [Required(ErrorMessage = "Room Incharge Name is required")]
        public string? RoomInChargeName { get; set; }
        public string? groupID { get; set; }
        public List<string>? Evaluations { get; set; }
        public string? EvaluationID { get; set; }
        public string? Member1ID { get; set; }
        public string? Member2ID { get; set; }
        [Required(ErrorMessage = "Room No is required")]
        public string? Room { get; set; }
        public DateTime AppointedDate { get; set; }
        public TimeOnly AppointedTime { get; set; }
        public string? Lapse { get; set; }
        public string? RoomIncharge { get; set; }

        public string? batch { get; set; }
        public string? istrue { get; set; }
        public string? assignRequest { get; set; }

        public List<FetchGroupListViewModel> studentGroups { get; set; }
        public List<FetchSupervisorListViewModel> SupervisorList1 { get; set; }
        public List<FetchSupervisorListViewModel> SupervisorList2 { get; set; }
        public List<string> batches { get; set; }

        public List<RoomViewModel> Rooms { get; set; }
        public List<RoomInChargeViewModel> roomInCharges { get; set; }
    }

    public class RoomViewModel
    {
        public string? Id { get; set; }
        public string? RoomNo { get; set; }

    }
    public class FetchGroupListViewModel
    {
        public string? groupID { get; set; }
        public string? groupName { get; set; }
        public string? ProjectName { get; set; }
        public string? Batch { get; set; }

    }
    public class FetchSupervisorListViewModel
    {
        public string? SupervisorID { get; set; }
        public string? SupervisorName { get; set; }

    }
    public class SupervisorsViewModel
    {
        public string UserId { get; set; }
        public string name { get; set; }
    }

    public class BatchViewModel
    {
        public string Batch { get; set; }
    }
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is Required")]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is Required")]

        public string? Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string? Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Enter Valid Email Address.")]
        public string? Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Required(ErrorMessage = "Password don't Match")]
        [Compare("Password", ErrorMessage = "Password don't Match!")]
        public string? ConfirmPassword { get; set; }


        [Required(ErrorMessage = "CNIC is required")]
        [RegularExpression(@"^\d{5}-\d{7}-\d{1}$", ErrorMessage = "CNIC must be in the format 12345-1234567-1")]
        public string? Cnic { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public string? Department { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot be longer than 200 characters")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Telephone is required")]
        [Phone(ErrorMessage = "Please enter a valid telephone number")]
        [StringLength(15, ErrorMessage = "Telephone cannot be longer than 15 characters")]
        public string? Telephone { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }
        public IEnumerable<DesignationViewModel> DesignationList { get; set; }

        public string Docs { get; set; }
        public string Designation { get; set; }

    }
    public class WeeklyLogsViewModel
    {
        public string? GroupId { get; set; }
        public string? WeekNo { get; set; }
        public string? GroupName { get; set; }
        public string? RoomNo { get; set; }
        public string? CurrentProject { get; set; }
        public string? CurrentPropDate { get; set; }
        public string? CurrentMidDate { get; set; }

        public string? Summary { get; set; }

        public string? Activities { get; set; }

        public string? Result { get; set; }

        public string? Tasks { get; set; }
        public string? ProjectStatus { get; set; }

        public DateTime AssignDate { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime currentDate { get; set; }
        public string? Status { get; set; }
        public DateTime UserSubmissionDate { get; set; }

        public List<WeeklyLogsViewModel> weeklies { get; set; }
    }

    public class ProjectViewModel
    {
        public Guid Id { get; set; }
        public string ProId { get; set; }
        public string code { get; set; }
        public string Title { get; set; }
        public string ProjectCategory { get; set; }
        public string projectGroup { get; set; }
        public string Others { get; set; }
        public string groupId { get; set; }
        public string groupname { get; set; }
        public string supervisorname { get; set; }
        public string cosupervisor { get; set; }
        public List<AppUserViewModel> cosupervisorList { get; set; }
        public string Specialization { get; set; }
        public string Tools { get; set; }
        public string companyID { get; set; }
        public string companyName { get; set; }
        public string companyMentor { get; set; }
        public string mentorcontact { get; set; }
        public string mentoremail { get; set; }
        public string batch { get; set; }
        public List<string> batches { get; set; }
        public string Summary { get; set; }
        public int TotalMarks { get; set; }
        public string objectives { get; set; }
        public string ExpectedResults { get; set; }
        public string commiteeId { get; set; }
        public string? changeTitleFormStatus { get; set; }
        public string? SupervsiorApproved { get; set; }
        public string? Status { get; set; }
        public string? evaluationId { get; set; }
        public string? Proposal { get; set; }
        public DateTime Proposaldate { get; set; }
        public int Proposalmarks { get; set; }
        public string? Proposalbatch { get; set; }
        public string? final { get; set; }
        public DateTime finaldate { get; set; }
        public int finalmarks { get; set; }
        public string? finalbatch { get; set; }
        public string? mid { get; set; }
        public DateTime middate { get; set; }
        public int midmarks { get; set; }
        public string? midbatch { get; set; }
        public string? PropPPT { get; set; }
        public string? PropReport { get; set; }
        public string? MidPPT { get; set; }
        public string? MidReport { get; set; }
        public string? FinalDocs { get; set; }
        public string? Docs { get; set; }
        public Project CurrentProject { get; set; }
        public List<ProjectViewModel> projects { get; set; }
        public CompanyViewModel company { get; set; }


    }
    public class EvaluationViewModel
    {
        public Guid EvaluationID { get; set; }
        public string? PBatch { get; set; }
        public string? PID { get; set; }
        public int Marks { get; set; }

        public DateTime LastDate { get; set; }

        public string? EvaluationName { get; set; }
        public List<string> batches { get; set; }
        public List<string> EvalType { get; set; }
        public List<EvaluationViewModel> evaluations { get; set; }
        public string? PropDocs { get; set; }
        public string? MidDocs { get; set; }
        public string? FinalDocs { get; set; }
        public int TotalMarks { get; set; }




    }
    public class AppUserViewModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        public string? areaofintrest { get; set; }

        [StringLength(15, MinimumLength = 15, ErrorMessage = "CNIC must be exactly 13 characters.")]
        [Required(ErrorMessage = "CNIC is required")]
        [RegularExpression(@"^\d{5}-\d{7}-\d{1}$", ErrorMessage = "CNIC must be in the format 12345-1234567-1")]
        public string? Cnic { get; set; }

        [StringLength(100, ErrorMessage = "Department cannot be longer than 100 characters.")]
        [Required(ErrorMessage = "Department is required")]
        public string? Department { get; set; }

        [StringLength(250, ErrorMessage = "Address cannot be longer than 250 characters.")]
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Invalid telephone number.")]
        [Required(ErrorMessage = "Telephone is required")]

        public string? Telephone { get; set; }

        [StringLength(50, ErrorMessage = "Role cannot be longer than 50 characters.")]
        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? email { get; set; }

        public string Docs { get; set; }

        [Required(ErrorMessage = "Designation is required.")]
        public string Designation { get; set; }
        public string DesignationId { get; set; }
        public IEnumerable<DesignationViewModel> DesignationList { get; set; }
        public AppUserViewModel appUser { get; set; }
        public SupervisorViewModel supervisor { get; set; }
        public string supervisorStudent { get; set; }
        public string groupname { get; set; }
        public string LeaderName { get; set; }
        public string supervisorEmail { get; set; }
        public StudentGroupViewModel StudentGroup { get; set; }
        public string? Enrollment { get; set; }
        public int regNo { get; set; }
        public string? semester { get; set; }
        public string? group1name { get; set; }
        public string? group2name { get; set; }
        public string? group1leader { get; set; }
        public string? group2leader { get; set; }


    }
    public class SupervisorViewModel
    {
        public string Id { get; set; }
        public string SupervisorID { get; set; }
        public string Designation { get; set; }
    }
    public class DesignationViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class CompanyViewModel
    {
        public string Id { get; set; }
        public string companyName { get; set; }
        public string companyMentor { get; set; }
        public string mentorcontact { get; set; }
        public string mentoremail { get; set; }
    }
    public class EvaluationCriteriaViewModel
    {
        public string Id { get; set; }
        public List<string> EvalType { get; set; }
        public string? PId { get; set; }
        public string? EvalName { get; set; }
        public string? Q1 { get; set; }
        public string? Q1Desc { get; set; }
        public string? Q1Marks { get; set; }
        public string? Q2 { get; set; }
        public string? Q2Desc { get; set; }

        public string? Q2Marks { get; set; }
        public string? Q3 { get; set; }
        public string? Q3Desc { get; set; }

        public string? Q3Marks { get; set; }
        public string Q4 { get; set; }
        public string? Q4Desc { get; set; }

        public string? Q4Marks { get; set; }
        public string? Q5 { get; set; }
        public string? Q5Desc { get; set; }

        public string? Q5Marks { get; set; }
        public string? Remarks { get; set; }
        public string? CommiteeID { get; set; }

        public int TotalMarks { get; set; }


    }

}
