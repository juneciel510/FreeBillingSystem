using System.ComponentModel.DataAnnotations;

namespace FreeBilling.Web.Pages.Models
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [MinLength(25)]
        public string Body { get; set; } = string.Empty;
    }
}
