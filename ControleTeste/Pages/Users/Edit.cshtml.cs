using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ControleTeste.Models;
using Microsoft.AspNetCore.Authorization;
using ControleTeste.Services;

namespace ControleTeste.Pages.Users
{
    [Authorize(Policy = "IsAdmin")]
    public class EditModel : PageModel
    {
        private readonly IUserService _service;
        public EditModel(IUserService service)
        {
            _service = service;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();

            Input = new InputModel
            {
                Id = user.Id,
                Username = user.Username,
                IsAdmin = user.IsAdmin
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var updated = await _service.UpdateAsync(Input.Id, Input.Username, Input.Password, Input.IsAdmin);
            if (!updated) return NotFound();

            return RedirectToPage("Index");
        }
    }
}
