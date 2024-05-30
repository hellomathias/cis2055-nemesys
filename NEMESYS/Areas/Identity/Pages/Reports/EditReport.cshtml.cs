using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NEMESYS.Areas.Identity.Data;
using NEMESYS.Data;
using NEMESYS.Areas.Identity.Pages.Reports.Models;

namespace NEMESYS.Areas.Identity.Pages.Reports
{
    public class EditReportModel : PageModel
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EditReportModel> _logger;

        public EditReportModel(AuthDbContext context, UserManager<ApplicationUser> userManager, ILogger<EditReportModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public Report Report { get; set; }

        [BindProperty]
        public IFormFile? Photo { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            _logger.LogInformation("Fetching the report with ID {ReportId}", id);

            Report = await _context.Reports.FindAsync(id);

            if (Report == null)
            {
                _logger.LogWarning("Report with ID {ReportId} not found", id);
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("User is not authenticated");
                return Challenge(); // Redirects to login page
            }

            _logger.LogInformation("Report's UserId: {ReportUserId}, Current UserId: {CurrentUserId}", Report.UserId, user.Id);

            if (Report.UserId != user.Id)
            {
                _logger.LogWarning("User ID {UserId} does not own the report", user.Id);
                return Forbid();
            }

            // Explicitly set the status here
            ViewData["CurrentStatus"] = Report.Status;

            _logger.LogInformation("User ID {UserId} is authorized to edit the report", user.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Submitting changes for the report with ID {ReportId}", Report.ReportId);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state is invalid.");
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"{state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("User is not authenticated");
                return Challenge();
            }

            _logger.LogInformation("Report's UserId: {ReportUserId}, Current UserId: {CurrentUserId}", Report.UserId, user.Id);

            if (Report.UserId != user.Id)
            {
                _logger.LogWarning("User ID {UserId} does not own the report", user.Id);
                return Forbid();
            }

            var reportToUpdate = await _context.Reports.FindAsync(Report.ReportId);
            if (reportToUpdate == null)
            {
                _logger.LogWarning("Report with ID {ReportId} not found", Report.ReportId);
                return NotFound();
            }

            // Preserve status
            var currentStatus = reportToUpdate.Status;

            // Update report fields
            reportToUpdate.DateOfReport = Report.DateOfReport;
            reportToUpdate.Location = Report.Location;
            reportToUpdate.DateSpotted = Report.DateSpotted;
            reportToUpdate.TimeSpotted = Report.TimeSpotted;
            reportToUpdate.TypeOfHazard = Report.TypeOfHazard;
            reportToUpdate.Description = Report.Description;
            reportToUpdate.Status = Report.Status ?? currentStatus; // Preserve status if not explicitly set
            reportToUpdate.ReporterEmail = Report.ReporterEmail;
            reportToUpdate.ReporterPhone = Report.ReporterPhone;
            reportToUpdate.UserId = Report.UserId;

            if (Photo != null && Photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(fileStream);
                }

                reportToUpdate.OptionalPhotoPath = "/uploads/" + uniqueFileName;
                _logger.LogInformation($"Photo saved at: {reportToUpdate.OptionalPhotoPath}");
            }

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Report updated successfully.");
                return RedirectToPage("./EditConfirmation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating the report.");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the report. Please try again.");
                return Page();
            }
        }

    }
}
