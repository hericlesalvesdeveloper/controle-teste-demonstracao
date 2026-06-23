using ControleTeste.DTOs;
using ControleTeste.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControleTeste.Pages.Alteracoes;

public class DetailsModel : PageModel
{
    private readonly ControleTeste.Services.IAlteracaoService _service;

    public DetailsModel(ControleTeste.Services.IAlteracaoService service)
    {
        _service = service;
    }

    public RespostaAlteracaoDto Alteracao { get; set; } = new RespostaAlteracaoDto(0, string.Empty, string.Empty, string.Empty, ControleTeste.Enums.TipoAlteracao.Correcao, ControleTeste.Enums.StatusAlteracao.Aberta, ControleTeste.Enums.SistemaAlteracao.FerrariERP, string.Empty, DateTime.Now, string.Empty);

    public async Task OnGetAsync(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item != null) Alteracao = item;
    }
}
