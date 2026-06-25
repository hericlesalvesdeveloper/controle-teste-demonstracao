using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ControleTeste.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControleTeste.Services;

namespace ControleTeste.Pages.Users
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IUserService _service;
        public DetailsModel(IUserService service)
        {
            _service = service;
        }

        public Usuario Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Usuario = await _service.GetByIdAsync(id);
            if (Usuario == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
