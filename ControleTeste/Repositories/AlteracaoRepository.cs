using ControleTeste.Context;
using ControleTeste.DTOs;
using ControleTeste.Enums;
using ControleTeste.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleTeste.Repositories;

public class AlteracaoRepository : IAlteracaoRepository
{
    private readonly ControleTesteContext _context;

    public AlteracaoRepository(ControleTesteContext context)
    {
        _context = context;
    }

    public async Task<RespostaAlteracaoDto> AddAsync(int numero, RequisicaoAlteracaoDto dto)
    {
        var alteracao = new Alteracao(numero, dto.Titulo, dto.Descricao, dto.Tipo, dto.Status, dto.Sistema, dto.MenuSistema ?? string.Empty, dto.DataAbertura, dto.Observacao ?? string.Empty);

        _context.Alteracoes.Add(alteracao);
        await _context.SaveChangesAsync();

        return MapToDto(alteracao);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var a = await _context.Alteracoes.FindAsync(id);
        if (a == null) throw new ControleTeste.Exceptions.NotFoundException($"Alteração com id {id} não encontrada.");
        _context.Alteracoes.Remove(a);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<RespostaAlteracaoDto>> GetAllAsync()
    {
        var list = await _context.Alteracoes.AsNoTracking().ToListAsync();
        return list.Select(MapToDto);
    }

    public async Task<RespostaAlteracaoDto?> GetByIdAsync(int id)
    {
        var a = await _context.Alteracoes.AsNoTracking().FirstOrDefaultAsync(x => x.AlteracaoId == id);
        return a == null ? null : MapToDto(a);
    }

    public async Task<bool> UpdateAsync(int id, RequisicaoAlteracaoDto dto, int numero)
    {
        var a = await _context.Alteracoes.FindAsync(id);
        if (a == null) throw new ControleTeste.Exceptions.NotFoundException($"Alteração com id {id} não encontrada.");

        // atualizar campos usando métodos da entidade
        a.AtualizarDados(numero, dto.Titulo, dto.Descricao, dto.Tipo, dto.Sistema, dto.MenuSistema);
        a.AtualizarObservacao(dto.Observacao);

        _context.Alteracoes.Update(a);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ChangeStatusAsync(int id, StatusAlteracao status, string? observacao)
    {
        var a = await _context.Alteracoes.FindAsync(id);
        if (a == null) throw new ControleTeste.Exceptions.NotFoundException($"Alteração com id {id} não encontrada.");

        // valida e aplica via método da entidade
        a.AlterarStatus(status, observacao);
        _context.Alteracoes.Update(a);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<RespostaAlteracaoDto>> GetByStatusAsync(StatusAlteracao status)
    {
        var list = await _context.Alteracoes.AsNoTracking().Where(x => x.Status == status).ToListAsync();
        return list.Select(MapToDto);
    }

    public async Task<IEnumerable<RespostaAlteracaoDto>> GetWithObservacaoAsync()
    {
        var list = await _context.Alteracoes.AsNoTracking().Where(x => !string.IsNullOrEmpty(x.Observacao)).ToListAsync();
        return list.Select(MapToDto);
    }

    public async Task<ControleTeste.DTOs.PagedResult<RespostaAlteracaoDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, StatusAlteracao? status, SistemaAlteracao? sistema)
    {
        var query = _context.Alteracoes.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.ToLower();
            query = query.Where(a => a.Titulo.ToLower().Contains(term) || a.NumeroAlteracao.ToString().Contains(term));
        }

        if (status.HasValue)
            query = query.Where(a => a.Status == status.Value);

        if (sistema.HasValue)
            query = query.Where(a => a.Sistema == sistema.Value);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(a => a.DataAbertura)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ControleTeste.DTOs.PagedResult<RespostaAlteracaoDto>(items.Select(MapToDto), total, pageNumber, pageSize);
    }

    public async Task<ControleTeste.DTOs.PagedResult<ControleTeste.DTOs.ReportRowDto>> GetReportAsync(int pageNumber, int pageSize, ControleTeste.DTOs.ReportFilterDto filter)
    {
        var query = _context.Alteracoes.AsNoTracking().AsQueryable();

        if (filter.DateFrom.HasValue)
            query = query.Where(a => a.DataAbertura >= filter.DateFrom.Value);
        if (filter.DateTo.HasValue)
            query = query.Where(a => a.DataAbertura <= filter.DateTo.Value);
        if (filter.Status.HasValue)
            query = query.Where(a => a.Status == filter.Status.Value);
        if (filter.Sistema.HasValue)
            query = query.Where(a => a.Sistema == filter.Sistema.Value);
        if (filter.Tipo.HasValue)
            query = query.Where(a => a.Tipo == filter.Tipo.Value);
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.ToLower();
            query = query.Where(a => a.Titulo.ToLower().Contains(term) || a.NumeroAlteracao.ToString().Contains(term));
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(a => a.DataAbertura)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new ControleTeste.DTOs.ReportRowDto(a.AlteracaoId, a.NumeroAlteracao, a.Titulo, a.Descricao, a.MenuSistema, a.Tipo, a.Status, a.Sistema, a.DataAbertura, a.Observacao))
            .ToListAsync();

        return new ControleTeste.DTOs.PagedResult<ControleTeste.DTOs.ReportRowDto>(items, total, pageNumber, pageSize);
    }

    public async Task<IEnumerable<ControleTeste.DTOs.ReportRowDto>> GetReportRowsAsync(ControleTeste.DTOs.ReportFilterDto filter, int maxRows)
    {
        var query = _context.Alteracoes.AsNoTracking().AsQueryable();

        if (filter.DateFrom.HasValue)
            query = query.Where(a => a.DataAbertura >= filter.DateFrom.Value);
        if (filter.DateTo.HasValue)
            query = query.Where(a => a.DataAbertura <= filter.DateTo.Value);
        if (filter.Status.HasValue)
            query = query.Where(a => a.Status == filter.Status.Value);
        if (filter.Sistema.HasValue)
            query = query.Where(a => a.Sistema == filter.Sistema.Value);
        if (filter.Tipo.HasValue)
            query = query.Where(a => a.Tipo == filter.Tipo.Value);
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.ToLower();
            query = query.Where(a => a.Titulo.ToLower().Contains(term) || a.NumeroAlteracao.ToString().Contains(term));
        }

        var items = await query
            .OrderByDescending(a => a.DataAbertura)
            .Take(maxRows)
            .Select(a => new ControleTeste.DTOs.ReportRowDto(a.AlteracaoId, a.NumeroAlteracao, a.Titulo, a.Descricao, a.MenuSistema, a.Tipo, a.Status, a.Sistema, a.DataAbertura, a.Observacao))
            .ToListAsync();

        return items;
    }

    private static RespostaAlteracaoDto MapToDto(Alteracao a)
    {
        return new RespostaAlteracaoDto(a.AlteracaoId, a.NumeroAlteracao.ToString(), a.Titulo, a.Descricao, a.Tipo, a.Status, a.Sistema, a.MenuSistema, a.DataAbertura, a.Observacao);
    }
}
