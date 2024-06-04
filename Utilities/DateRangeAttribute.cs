using System.ComponentModel.DataAnnotations;
using ASP_Labb1.Models;

public class DateRangeAttribute : ValidationAttribute
{
	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		var leaveApplication = (LeaveApplication)validationContext.ObjectInstance;

		if (leaveApplication.LeaveEndDate < leaveApplication.LeaveStartDate)
		{
			return new ValidationResult("Slutdatum måste vara efter startdatum.");
		}

		return ValidationResult.Success;
	}
}