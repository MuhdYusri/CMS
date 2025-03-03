using System;
using System.ComponentModel.DataAnnotations;

namespace CaseManagementAPI.Models
{
    public class CustomerCase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CaseChannel { get; set; } // AI, Call, WhatsApp, Email

        public string Status { get; set; } = "Open"; // Default: Open

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
