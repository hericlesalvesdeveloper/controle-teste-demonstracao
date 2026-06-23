using ControleTeste.DTOs;
using ControleTeste.Repositories;
using ControleTeste.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace ControleTeste.Pages.Alteracoes;

public class IndexModel : PageModel
{
    private readonly ControleTeste.Services.IAlteracaoService _service;

    public IndexModel(ControleTeste.Services.IAlteracaoService service)
    {
        _service = service;
    }

    public IEnumerable<RespostaAlteracaoDto> Alteracoes { get; set; } = Enumerable.Empty<RespostaAlteracaoDto>();

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 10;

    public int TotalItems { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public StatusAlteracao? StatusFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public SistemaAlteracao? SistemaFilter { get; set; }

    public async Task OnGetAsync()
    {
        PageNumber = PageNumber < 1 ? 1 : PageNumber;
        PageSize = PageSize <= 0 ? 10 : PageSize;

        var paged = await _service.GetPagedAsync(PageNumber, PageSize, Search, StatusFilter, SistemaFilter);
        Alteracoes = paged.Items;
        TotalItems = paged.TotalItems;
    }
}
