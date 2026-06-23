using ControleTeste.DTOs;
using ControleTeste.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControleTeste.Pages.Alteracoes;

public class CreateModel : PageModel
{
    private readonly ControleTeste.Services.IAlteracaoService _service;

    public CreateModel(ControleTeste.Services.IAlteracaoService service)
    {
        _service = service;
    }

    [BindProperty]
    public RequisicaoAlteracaoDto Alteracao { get; set; } = new RequisicaoAlteracaoDto("0", string.Empty, string.Empty, ControleTeste.Enums.TipoAlteracao.Correcao, ControleTeste.Enums.StatusAlteracao.Aberta, ControleTeste.Enums.SistemaAlteracao.FerrariERP, null, DateTime.Now, null);

    public void OnGet()
    {
        // inicializar se necessário
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            await _service.CreateAsync(Alteracao);
            return RedirectToPage("/Alteracoes/Index");
        }
        catch (ControleTeste.Exceptions.AppValidationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar a alteração.");
            return Page();
        }
    }
}
