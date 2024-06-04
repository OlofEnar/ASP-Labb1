using System.ComponentModel.DataAnnotations;

namespace ASP_Labb1.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Du måste skriva in ett giltigt namn")]
        [StringLength(50, MinimumLength = 2)]
        [Display (Name = "Förnamn")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Du måste skriva in ett giltigt namn")]
		[StringLength(50, MinimumLength = 2)]
		[Display(Name = "Efternamn")]
		public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();
    }
}