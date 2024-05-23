using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NEMESYS.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NEMESYS.Areas.Identity.Data;
using NEMESYS.Areas.Identity.Pages.Reports.Models;

namespace NEMESYS.Areas.Identity.Pages.Reports
{
    public class ReportPageModel : PageModel
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReportPageModel> _logger;

        public ReportPageModel(AuthDbContext context, UserManager<ApplicationUser> userManager, ILogger<ReportPageModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public Report CurrentReport { get; set; }

        [BindProperty]
        public IFormFile? Photo { get; set; }

        public void OnGet()
        {
            CurrentReport = new Report
            {
                DateOfReport = DateTime.Today,
                Status = "Open"
            };
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
            if (user == null)
            {
                _logger.LogError("User is not logged in.");
                return Challenge();
            }

            _logger.LogInformation($"User ID: {user.Id}");
            CurrentReport.UserId = user.Id;

            if (Photo != null && Photo.Length > 0)
            {
                _logger.LogInformation($"Photo uploaded: {Photo.FileName}");
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Photo.CopyToAsync(fileStream);
                }

                CurrentReport.OptionalPhotoPath = "/uploads/" + uniqueFileName;
                _logger.LogInformation($"Photo saved at: {CurrentReport.OptionalPhotoPath}");
            }
            else
            {
                _logger.LogWarning("No photo uploaded or photo is empty.");
            }

            try
            {
                _context.Reports.Add(CurrentReport);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Report saved successfully.");
                return RedirectToPage("./Confirmation");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving the report.");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the report. Please try again.");
                return Page();
            }
        }
    }
}
