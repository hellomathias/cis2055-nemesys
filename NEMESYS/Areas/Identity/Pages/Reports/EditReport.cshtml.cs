using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
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
            Report = await _context.Reports.FindAsync(id);

            if (Report == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || Report.UserId != user.Id)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
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
            if (user == null || Report.UserId != user.Id)
            {
                return Forbid();
            }

            var reportToUpdate = await _context.Reports.FindAsync(Report.ReportId);
            if (reportToUpdate == null)
            {
                return NotFound();
            }

            // Update report fields
            reportToUpdate.DateOfReport = Report.DateOfReport;
            reportToUpdate.Location = Report.Location;
            reportToUpdate.DateSpotted = Report.DateSpotted;
            reportToUpdate.TimeSpotted = Report.TimeSpotted;
            reportToUpdate.TypeOfHazard = Report.TypeOfHazard;
            reportToUpdate.Description = Report.Description;
            reportToUpdate.Status = Report.Status;
            reportToUpdate.ReporterEmail = Report.ReporterEmail;
            reportToUpdate.ReporterPhone = Report.ReporterPhone;

            if (Photo != null && Photo.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

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
                return RedirectToPage("./ReportBoard");
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
