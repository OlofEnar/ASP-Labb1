using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_Labb1.Models
{
	public enum LeaveType
	{
		[Display(Name = "Sjukdom")]
		Illness,

		[Display(Name = "Semester")]
		Vacation,

		[Display(Name = "Vård av barn")]
		ParentalReasons,

		[Display(Name = "Other")]
		Other
	}

	public class LeaveApplication
	{
		[Key]
		public int LeaveId { get; set; }

		[Display(Name = "Startdatum")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		[Required(ErrorMessage = "Du måste välja ett startdatum")]
		public DateTime LeaveStartDate { get; set; }

		[Display(Name = "Slutdatum")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		[Required(ErrorMessage = "Du måste välja ett slutdatum")]
		[DateRange(ErrorMessage = "Slutdatum kan inte vara tidigare än startdatum.")]
		public DateTime LeaveEndDate { get; set; }

		[Display(Name = "Ansöksdatum")]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime LeaveApplicationDate { get; set; } = DateTime.Now;

		[Display(Name = "Orsak")]
		[Required(ErrorMessage = "Du måste välja en orsak")]
		public LeaveType? LeaveType { get; set; }

		[ForeignKey("Employee")]
		public int FkEmployeeId { get; set; }

		// navigation
		public Employee? Employee { get; set; }

		public int TotalDays
		{
			get
			{
				return (LeaveEndDate - LeaveStartDate).Days + 1;
			}
		}
	}
}