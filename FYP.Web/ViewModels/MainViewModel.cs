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
        public Guid Id;
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [MaxLength(50) , MinLength(5)]
        public string Name { get; set; }
        public string student1LID { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string student1LEmail { get; set; }
        public string student2ID { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string student2Email { get; set; }
        public string student3ID { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string student3Email { get; set; }
        public string Batch { get; set; }
        public string Year { get; set; }
        public string SupervisorID { get; set; }
        public string CoSupervisorID { get; set; }
        public string companyID { get; set; }
        public string CordinatorID { get; set; }
        public string projectname { get; set; }
        public IEnumerable<StudentGroupViewModel> StudentGroups { get; set; }


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
    public class ProjectViewModel
    {
        public Guid Id { get; set; }
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
        public string Summary { get; set; }
        public string objectives { get; set; }
        public string ExpectedResults { get; set; }
        public string commiteeId { get; set; }
        public bool Status { get; set; }
        public List<ProjectViewModel> projects { get; set; }
        public CompanyViewModel company { get; set; }


    }
    public class AppUserViewModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; }

        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

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
}
