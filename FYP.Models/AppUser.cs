using Microsoft.AspNetCore.Identity;

namespace FYP.Entities
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Cnic { get; set; }
        public string? Department { get; set; }
        public string? Address { get; set; }
        public string? Designation { get; set; }
        public string? Telephone { get; set; }
        public string? Role { get; set; } // This could be redundant as Identity already handles roles

        public string? Docs { get; set; }
    }
}
