using ControleTeste.DTOs;
using ControleTeste.Enums;

namespace ControleTeste.Services;

public interface IAlteracaoService
{
    Task<IEnumerable<RespostaAlteracaoDto>> GetAllAsync();
    Task<RespostaAlteracaoDto?> GetByIdAsync(int id);
    Task<RespostaAlteracaoDto> CreateAsync(RequisicaoAlteracaoDto dto);
    Task UpdateAsync(int id, RequisicaoAlteracaoDto dto);
    Task DeleteAsync(int id);
    Task ChangeStatusAsync(int id, StatusAlteracao status, string? observacao);
    Task<IEnumerable<RespostaAlteracaoDto>> GetByStatusAsync(StatusAlteracao status);
    Task<IEnumerable<RespostaAlteracaoDto>> GetWithObservacaoAsync();
    Task<PagedResult<RespostaAlteracaoDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, StatusAlteracao? status, SistemaAlteracao? sistema);
    Task<PagedResult<ReportRowDto>> GetReportAsync(int pageNumber, int pageSize, ReportFilterDto filter);
    Task<IEnumerable<ReportRowDto>> GetReportRowsAsync(ReportFilterDto filter, int maxRows);
}
