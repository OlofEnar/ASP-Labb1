using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP_Labb1.Data;
using ASP_Labb1.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using ASP_Labb1.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_Labb1.Controllers
{
	public class LeaveApplicationsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public LeaveApplicationsController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(int? month)
		{
			var leaveApplications = _context.LeaveApplications
				.Include(l => l.Employee)
				.AsQueryable();

			if (month.HasValue)
			{
				leaveApplications = leaveApplications.Where(l => l.LeaveStartDate.Month == month);
			}

			var leaveApplicationsList = await leaveApplications.ToListAsync();

			ViewBag.SelectedMonth = month;
			ViewBag.Months = Enumerable.Range(1, 12)
				.Select(m => new SelectListItem
				{
					Value = m.ToString(),
					Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
				}).ToList();

			return View(leaveApplicationsList);
		}

        // GET: Leaves by employee id
        public async Task<IActionResult> GetLeavesByEmpId(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplications = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Where(l => l.FkEmployeeId == id)
                .ToListAsync();

            return View(leaveApplications);
        }


		// GET: LeaveApplications/Create
		public async Task<IActionResult> Create(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var employee = await _context.Employees.FindAsync(id);
			if (employee == null)
			{
				return NotFound();
			}

			ViewBag.EmployeeFullName = employee.FullName;
			ViewData["LeaveTypes"] = Enum.GetValues(typeof(LeaveType))
				.Cast<LeaveType>()
				.Select(lt => new SelectListItem
				{
					Text = EnumExtensions.GetDisplayName(lt),
					Value = ((int)lt).ToString()
				});

			var leaveApplication = new LeaveApplication
			{
				FkEmployeeId = employee.EmployeeId,
				LeaveStartDate = DateTime.Today,
				LeaveEndDate = DateTime.Today
			};

			return View(leaveApplication);
		}


		// POST: LeaveApplications/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(LeaveApplication leaveApplication)
		{
			if (ModelState.IsValid)
			{
				_context.Add(leaveApplication);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			var employee = await _context.Employees.FindAsync(leaveApplication.FkEmployeeId);
			if (employee != null)
			{
				ViewBag.EmployeeFullName = employee.FullName;
			}

			ViewBag.LeaveTypes = Enum.GetValues(typeof(LeaveType)).Cast<LeaveType>().ToList();
			return View(leaveApplication);
		}

		// GET: LeaveApplications/Edit/5
		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var leaveApplication = await _context.LeaveApplications
				.Include(l => l.Employee)
				.FirstOrDefaultAsync(m => m.LeaveId == id);

			if (leaveApplication == null)
			{
				return NotFound();
			}

			ViewBag.EmployeeFullName = leaveApplication.Employee?.FullName;
			ViewData["LeaveTypes"] = Enum.GetValues(typeof(LeaveType))
				.Cast<LeaveType>()
				.Select(lt => new SelectListItem
				{
					Text = EnumExtensions.GetDisplayName(lt),
					Value = ((int)lt).ToString(),
					Selected = (int)leaveApplication.LeaveType == (int)lt
				});

			return View(leaveApplication);
		}

		// POST: LeaveApplications/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, LeaveApplication leaveApplication)
		{
			if (id != leaveApplication.LeaveId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(leaveApplication);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!LeaveApplicationExists(leaveApplication.LeaveId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}

			var employee = await _context.Employees.FindAsync(leaveApplication.FkEmployeeId);
			if (employee != null)
			{
				ViewBag.EmployeeFullName = employee.FullName;
			}

			ViewData["LeaveTypes"] = Enum.GetValues(typeof(LeaveType))
				.Cast<LeaveType>()
				.Select(lt => new SelectListItem
				{
					Text = EnumExtensions.GetDisplayName(lt),
					Value = ((int)lt).ToString(),
					Selected = (int)leaveApplication.LeaveType == (int)lt
				});

			return View(leaveApplication);
		}


		// GET: LeaveApplications/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var leaveApplication = await _context.LeaveApplications
				.FirstOrDefaultAsync(m => m.LeaveId == id);
			if (leaveApplication == null)
			{
				return NotFound();
			}

			return View(leaveApplication);
		}

		// POST: LeaveApplications/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var leaveApplication = await _context.LeaveApplications.FindAsync(id);
			if (leaveApplication != null)
			{
				_context.LeaveApplications.Remove(leaveApplication);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool LeaveApplicationExists(int id)
		{
			return _context.LeaveApplications.Any(e => e.LeaveId == id);
		}
	}
}
