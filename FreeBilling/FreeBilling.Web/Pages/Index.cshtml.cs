using FreeBilling.Data.Entities;
using FreeBilling.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FreeBilling.Web.Pages
{
    public class IndexModel : PageModel
    {
        private IBillingRepository _repository;
        public IEnumerable<Customer>? Customers { get; set; }

        public IndexModel(IBillingRepository repository)
        {
            _repository = repository;

        }
        public async Task OnGetAsync()
        {
            Customers = await _repository.GetCustomers();
        }
    }
}
