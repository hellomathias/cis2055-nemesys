using NEMESYS.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NEMESYS.Areas.Identity.Pages.Reports.Models
{
    public class Upvote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ReportId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("ReportId")]
        public Report Report { get; set; }
    }
}
