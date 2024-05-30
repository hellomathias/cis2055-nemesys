using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEMESYS.Areas.Identity.Pages.Investigations.Models
{
    public class Investigation
    {
        [Key]
        public int InvestigationId { get; set; }

        [Required]
        public int ReportId { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Description is too long.")]
        public string Description { get; set; }

        [Required]
        public DateTime DateOfAction { get; set; }

        [Required]
        public string InvestigatorId { get; set; }

        [EmailAddress]
        public string InvestigatorEmail { get; set; }

        [Phone]
        public string InvestigatorPhone { get; set; }
    }
}
