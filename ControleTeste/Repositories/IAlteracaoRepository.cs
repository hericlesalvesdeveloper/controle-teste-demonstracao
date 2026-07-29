using ControleTeste.DTOs;
using ControleTeste.Enums;

namespace ControleTeste.Repositories;

public interface IAlteracaoRepository
{
    Task<IEnumerable<RespostaAlteracaoDto>> GetAllAsync();
    Task<RespostaAlteracaoDto?> GetByIdAsync(int id);
    // número já deve estar validado/parseado pelo service
    Task<RespostaAlteracaoDto> AddAsync(int numero, RequisicaoAlteracaoDto dto);
    Task<bool> UpdateAsync(int id, RequisicaoAlteracaoDto dto, int numero);
    Task<bool> DeleteAsync(int id);
    Task<bool> AlterarStatusAsync(int id, StatusAlteracao status, string? observacao);
    Task<IEnumerable<RespostaAlteracaoDto>> GetByStatusAsync(StatusAlteracao status);
    Task<IEnumerable<RespostaAlteracaoDto>> GetWithObservacaoAsync();
    Task<ControleTeste.DTOs.ResultadoPaginado<RespostaAlteracaoDto>> GetPagedAsync(int pageNumber, int pageSize, string? search, StatusAlteracao? status, SistemaAlteracao? sistema);
    Task<ControleTeste.DTOs.ResultadoPaginado<ControleTeste.DTOs.LinhaRelatorioDto>> GetReportAsync(int pageNumber, int pageSize, ControleTeste.DTOs.FiltroRelatorioDto filter);
    Task<IEnumerable<ControleTeste.DTOs.LinhaRelatorioDto>> GetReportRowsAsync(ControleTeste.DTOs.FiltroRelatorioDto filter, int maxRows);
}
