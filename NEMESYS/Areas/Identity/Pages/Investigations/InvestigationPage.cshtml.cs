using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NEMESYS.Areas.Identity.Data;
using NEMESYS.Areas.Identity.Pages.Reports.Models;
using NEMESYS.Data;
using System;
using System.Threading.Tasks;
using NEMESYS.Areas.Identity.Pages.Investigations.Models;

namespace NEMESYS.Areas.Identity.Pages.Investigations
{
    public class InvestigationPageModel : PageModel
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<InvestigationPageModel> _logger;

        public InvestigationPageModel(AuthDbContext context, UserManager<ApplicationUser> userManager, ILogger<InvestigationPageModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public Investigation CurrentInvestigation { get; set; }

        [BindProperty]
        public Report Report { get; set; }

        [BindProperty]
        public string SelectedStatus { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Report = await _context.Reports.FindAsync(id);
            if (Report == null)
            {
                _logger.LogWarning("Report with ID {ReportId} not found.", id);
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var investigator = await _userManager.GetUserAsync(User);

            // Check if an investigation already exists for this report and investigator
            CurrentInvestigation = await _context.Investigations
                .FirstOrDefaultAsync(i => i.ReportId == Report.ReportId && i.InvestigatorId == userId);

            if (CurrentInvestigation == null)
            {
                // Initialize new investigation if it doesn't exist
                CurrentInvestigation = new Investigation
                {
                    ReportId = Report.ReportId,
                    DateOfAction = DateTime.Now,
                    InvestigatorId = userId,
                    InvestigatorEmail = investigator.Email,
                    InvestigatorPhone = investigator.PhoneNumber
                };
            }

            SelectedStatus = Report.Status;  // Initialize the selected status with the current report status

            _logger.LogInformation("Report with ID {ReportId} found.", id);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Error in field {Field}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }

                return Page();
            }

            try
            {
                // Check if an investigation already exists for this report and investigator
                var existingInvestigation = await _context.Investigations
                    .FirstOrDefaultAsync(i => i.ReportId == CurrentInvestigation.ReportId && i.InvestigatorId == CurrentInvestigation.InvestigatorId);

                if (existingInvestigation != null)
                {
                    // Update the existing investigation
                    existingInvestigation.Description = CurrentInvestigation.Description;
                    existingInvestigation.DateOfAction = CurrentInvestigation.DateOfAction;
                    existingInvestigation.InvestigatorEmail = CurrentInvestigation.InvestigatorEmail;
                    existingInvestigation.InvestigatorPhone = CurrentInvestigation.InvestigatorPhone;
                    _context.Investigations.Update(existingInvestigation);
                }
                else
                {
                    // Add a new investigation
                    _context.Investigations.Add(CurrentInvestigation);
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Investigation saved for report {ReportId}.", CurrentInvestigation.ReportId);

                // Update report status
                var report = await _context.Reports.FindAsync(CurrentInvestigation.ReportId);
                if (report != null)
                {
                    report.Status = SelectedStatus;
                    _context.Reports.Update(report);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Report status updated to {Status} for report {ReportId}.", SelectedStatus, CurrentInvestigation.ReportId);
                }
                else
                {
                    _logger.LogWarning("Report with ID {ReportId} not found during status update.", CurrentInvestigation.ReportId);
                }

                return RedirectToPage("/Reports/ReportBoard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving investigation for report {ReportId}.", CurrentInvestigation.ReportId);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
