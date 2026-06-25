using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ControleTeste.Models;
using Microsoft.AspNetCore.Authorization;
using ControleTeste.Services;

namespace ControleTeste.Pages.Users
{
    [Authorize(Policy = "IsAdmin")]
    public class DeleteModel : PageModel
    {
        private readonly IUserService _service;
        public DeleteModel(IUserService service)
        {
            _service = service;
        }

        [BindProperty]
        public Usuario Usuario { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Usuario = await _service.GetByIdAsync(id);
            if (Usuario == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return RedirectToPage("Index");
        }
    }
}
