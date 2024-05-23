using System;
using System.ComponentModel.DataAnnotations;

namespace NEMESYS.Areas.Identity.Pages.Reports.Models
{
    public class Report
    {
        public int ReportId { get; set; }

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
        [StringLength(1000, ErrorMessage = "Description is too long.")]
        public string Description { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Status is too long.")]
        public string Status { get; set; } = "Open";

        [Required]
        [EmailAddress]
        public string ReporterEmail { get; set; }

        [Phone]
        public string? ReporterPhone { get; set; }

        public string? OptionalPhotoPath { get; set; }

        public string? UserId { get; set; } 

        public int Upvotes { get; set; }
    }
}
