using ControleTeste.DTOs;
using ControleTeste.Enums;
using ControleTeste.Repositories;

namespace ControleTeste.Services;

public class AlteracaoService : IAlteracaoService
{
    private readonly IAlteracaoRepository _repo;

    public AlteracaoService(IAlteracaoRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<RespostaAlteracaoDto>> GetAllAsync() => _repo.GetAllAsync();

    public Task<RespostaAlteracaoDto?> GetByIdAsync(int id) => _repo.GetByIdAsync(id);

    public Task<RespostaAlteracaoDto> CreateAsync(RequisicaoAlteracaoDto dto)
    {
        if (!int.TryParse(dto.NumeroAlteracao, out var numero))
            throw new ControleTeste.Exceptions.AppValidationException("NumeroAlteracao inválido. Deve ser numérico.");

        return _repo.AddAsync(numero, dto);
    }

    public Task UpdateAsync(int id, RequisicaoAlteracaoDto dto)
    {
        if (!int.TryParse(dto.NumeroAlteracao, out var numero))
            throw new ControleTeste.Exceptions.AppValidationException("NumeroAlteracao inválido. Deve ser numérico.");

        return _repo.UpdateAsync(id, dto, numero);
    }

    public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

    public Task AlterarStatusAsync(int id, StatusAlteracao status, string? observacao) => _repo.AlterarStatusAsync(id, status, observacao);

    public Task<IEnumerable<RespostaAlteracaoDto>> GetByStatusAsync(StatusAlteracao status) => _repo.GetByStatusAsync(status);

    public Task<IEnumerable<RespostaAlteracaoDto>> GetWithObservacaoAsync() => _repo.GetWithObservacaoAsync();

    public Task<ControleTeste.DTOs.ResultadoPaginado<RespostaAlteracaoDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, StatusAlteracao? status, SistemaAlteracao? sistema)
        => _repo.GetPagedAsync(pageNumber, pageSize, search, status, sistema);

    public Task<ControleTeste.DTOs.ResultadoPaginado<ControleTeste.DTOs.LinhaRelatorioDto>> GetReportAsync(int pageNumber, int pageSize, ControleTeste.DTOs.FiltroRelatorioDto filter)
        => _repo.GetReportAsync(pageNumber, pageSize, filter);

    public Task<IEnumerable<ControleTeste.DTOs.LinhaRelatorioDto>> GetReportRowsAsync(ControleTeste.DTOs.FiltroRelatorioDto filter, int maxRows)
        => _repo.GetReportRowsAsync(filter, maxRows);
}
