using ControleTeste.DTOs;
using ControleTeste.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControleTeste.Pages.Alteracoes;

public class EditModel : PageModel
{
    private readonly ControleTeste.Services.IAlteracaoService _service;

    public EditModel(ControleTeste.Services.IAlteracaoService service)
    {
        _service = service;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public RequisicaoAlteracaoDto Alteracao { get; set; } = new RequisicaoAlteracaoDto("0", string.Empty, string.Empty, ControleTeste.Enums.TipoAlteracao.Correcao, ControleTeste.Enums.StatusAlteracao.Aberta, ControleTeste.Enums.SistemaAlteracao.FerrariERP, null, DateTime.Now, null);

    public async Task<IActionResult> OnGetAsync()
    {
        var existing = await _service.GetByIdAsync(Id);
        if (existing == null) return RedirectToPage("/Alteracoes/Index");

        Alteracao = new RequisicaoAlteracaoDto(existing.NumeroAlteracao, existing.Titulo, existing.Descricao, existing.Tipo, existing.Status, existing.Sistema, existing.MenuSistema, existing.DataAbertura, existing.Observacao);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            await _service.UpdateAsync(Id, Alteracao);
            return RedirectToPage("/Alteracoes/Index");
        }
        catch (ControleTeste.Exceptions.AppValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (ControleTeste.Exceptions.NotFoundException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a alteração.");
            return Page();
        }
    }
}
