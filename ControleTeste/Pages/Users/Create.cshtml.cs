using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ControleTeste.Models;
using Microsoft.AspNetCore.Authorization;
using ControleTeste.Services;

namespace ControleTeste.Pages.Users
{
    [Authorize(Policy = "IsAdmin")]
    public class CreateModel : PageModel
    {
        private readonly IUserService _service;

        public CreateModel(IUserService service)
        {
            _service = service;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(50, MinimumLength = 3)]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6)]
            public string Password { get; set; }

            public bool IsAdmin { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _service.CreateAsync(Input.Username, Input.Password, Input.IsAdmin);

            return RedirectToPage("Index");
        }
    }
}
