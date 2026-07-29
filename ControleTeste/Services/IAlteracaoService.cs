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
    Task AlterarStatusAsync(int id, StatusAlteracao status, string? observacao);
    Task<IEnumerable<RespostaAlteracaoDto>> GetByStatusAsync(StatusAlteracao status);
    Task<IEnumerable<RespostaAlteracaoDto>> GetWithObservacaoAsync();
    Task<ResultadoPaginado<RespostaAlteracaoDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, StatusAlteracao? status, SistemaAlteracao? sistema);
    Task<ResultadoPaginado<LinhaRelatorioDto>> GetReportAsync(int pageNumber, int pageSize, FiltroRelatorioDto filter);
    Task<IEnumerable<LinhaRelatorioDto>> GetReportRowsAsync(FiltroRelatorioDto filter, int maxRows);
}
