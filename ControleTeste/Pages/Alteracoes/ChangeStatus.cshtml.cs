using ControleTeste.Enums;
using ControleTeste.DTOs;
using ControleTeste.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace ControleTeste.Pages.Alteracoes;

[Authorize]
public class ChangeStatusModel : PageModel
{
    private readonly ControleTeste.Services.IAlteracaoService _service;

    public ChangeStatusModel(ControleTeste.Services.IAlteracaoService service)
    {
        _service = service;
    }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    [BindProperty]
    public StatusAlteracao Status { get; set; } = StatusAlteracao.Aberta;

    [BindProperty]
    public string? Observacao { get; set; }

    public RespostaAlteracaoDto? AlteracaoAtual { get; set; }

    public async Task OnGetAsync()
    {
        var a = await _service.GetByIdAsync(Id);
        if (a == null)
        {
            // redireciona para index se não encontrar
            RedirectToPage("/Alteracoes/Index");
            return;
        }

        AlteracaoAtual = a;
        Status = a.Status;
        Observacao = a.Observacao;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _service.ChangeStatusAsync(Id, Status, Observacao);
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
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Ocorreu um erro ao alterar o status.");
            return Page();
        }
    }
}
