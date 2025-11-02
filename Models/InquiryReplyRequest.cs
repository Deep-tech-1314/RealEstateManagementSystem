using System.ComponentModel.DataAnnotations;

namespace RealEstateManagementSystem.Models
{
    public class InquiryReplyRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Reply { get; set; } = string.Empty;
    }
}

