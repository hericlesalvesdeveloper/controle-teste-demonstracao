using ControleTeste.Enums;

namespace ControleTeste.DTOs;

public class FiltroRelatorioDto
{
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public StatusAlteracao? Status { get; set; }
    public SistemaAlteracao? Sistema { get; set; }
    public TipoAlteracao? Tipo { get; set; }
    public string? Pesquisa { get; set; }
    public string? OrdenarPor { get; set; }

    public FiltroRelatorioDto() { }
}
