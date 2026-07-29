using System.Linq;
using ControleTeste.DTOs;
using ControleTeste.Repositories;
using ControleTeste.Enums;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ControleTeste.Models;
using ControleTeste.Services;

namespace ControleTeste.Pages.Alteracoes;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IAlteracaoService _service;
    private readonly IUserService _userService;

    public IndexModel(IAlteracaoService service, IUserService userService)
    {
        _service = service;
        _userService = userService;
    }

    public IEnumerable<RespostaAlteracaoDto> Alteracoes { get; set; } = Enumerable.Empty<RespostaAlteracaoDto>();

    public IEnumerable<Usuario> RecentUsers { get; set; } = Enumerable.Empty<Usuario>();

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 10;

    public int TotalItems { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Pesquisa { get; set; }

    [BindProperty(SupportsGet = true)]
    public StatusAlteracao? StatusFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public SistemaAlteracao? SistemaFilter { get; set; }

    public async Task OnGetAsync()
    {
        PageNumber = PageNumber < 1 ? 1 : PageNumber;
        PageSize = PageSize <= 0 ? 10 : PageSize;

        var paged = await _service.GetPagedAsync(PageNumber, PageSize, Pesquisa, StatusFilter, SistemaFilter);
        Alteracoes = paged.Itens;
        TotalItems = paged.TotalItens;

        var users = await _userService.GetAllAsync();
        RecentUsers = users.Take(5);
    }
}
