using ControleTeste.DTOs;
using ControleTeste.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace ControleTeste.Pages.Alteracoes;

[Authorize]
public class DeleteModel : PageModel
{
    private readonly ControleTeste.Services.IAlteracaoService _service;

    public DeleteModel(ControleTeste.Services.IAlteracaoService service)
    {
        _service = service;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public RespostaAlteracaoDto Alteracao { get; set; } = new RespostaAlteracaoDto(0, string.Empty, string.Empty, string.Empty, ControleTeste.Enums.TipoAlteracao.Correcao, ControleTeste.Enums.StatusAlteracao.Aberta, ControleTeste.Enums.SistemaAlteracao.FerrariERP, string.Empty, DateTime.Now, string.Empty);

    public async Task OnGetAsync()
    {
        var item = await _service.GetByIdAsync(Id);
        if (item != null) Alteracao = item;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _service.DeleteAsync(Id);
            return RedirectToPage("/Alteracoes/Index");
        }
        catch (ControleTeste.Exceptions.NotFoundException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao excluir a alteração.");
            return Page();
        }
    }
}
