using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FreeBilling.Web.Pages
{
    public class ContactModel : PageModel
    {
        //public ContactModel(IEmailService emailService)
        //{
        //    _emailService = emailService;
        //}

        public string Title { get; set; } = "Contact Me";
        public string Message { get; set; } = "";

        //[BindProperty]
        //public ContactViewModel Contact { get; set; } = new ContactViewModel()
        //{
        //    Name = "Shawn Wildermuth"
        //};
        public void OnGet()
        {
        }

        public void OnPost()
        {
            Message = "Not implemented";
        }
    }
}
