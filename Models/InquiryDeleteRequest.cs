using System.ComponentModel.DataAnnotations;

namespace RealEstateManagementSystem.Models
{
    public class InquiryDeleteRequest
    {
        [Required]
        public int Id { get; set; }
    }
}

