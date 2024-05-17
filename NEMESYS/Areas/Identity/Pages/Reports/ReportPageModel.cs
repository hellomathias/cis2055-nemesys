using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace NEMESYS.Areas.Identity.Pages.Reports
{
    [Authorize]
    public class ReportPageModel : PageModel
    {
        public Report CurrentReport { get; set; }

        public class Report
        {
            [Required]
            [StringLength(100, ErrorMessage = "Title is too long.")]
            public string Title { get; set; }

            [Required]
            [StringLength(1000, ErrorMessage = "Description is too long.")]
            public string Description { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime DateOfReport { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Location is too long.")]
            public string Location { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime DateSpotted { get; set; }

            [Required]
            [StringLength(10, ErrorMessage = "Time is too long.")]
            public string TimeSpotted { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "Type of hazard is too long.")]
            public string TypeOfHazard { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = "Status is too long.")]
            public string Status { get; set; }

            [Required]
            [EmailAddress]
            public string ReporterEmail { get; set; }

            [Phone]
            public string ReporterPhone { get; set; }

            public string OptionalPhotoPath { get; set; }
        }

        public void OnGet()
        {
            // Initialize CurrentReport with a default instance
            CurrentReport = new Report
            {
                Title = "Sample Report",
                Description = "Sample description",
                DateOfReport = DateTime.Now,
                Location = "Sample Location",
                DateSpotted = DateTime.Now,
                TimeSpotted = "12:00 PM",
                TypeOfHazard = "Sample Hazard",
                Status = "Open",
                ReporterEmail = "sample@example.com",
                ReporterPhone = "123-456-7890",
                OptionalPhotoPath = null
            };
        }
    }
}
