using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ControleTeste.Models;
using Microsoft.AspNetCore.Authorization;
using ControleTeste.Services;

namespace ControleTeste.Pages.Users
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUserService _service;
        public IndexModel(IUserService service)
        {
            _service = service;
        }

        public IList<Usuario> Usuarios { get; set; }

        public async Task OnGetAsync()
        {
            Usuarios = (IList<Usuario>)await _service.GetAllAsync();
        }
    }
}
