using ControleTeste.Enums;

namespace ControleTeste.DTOs;

public class ReportFilterDto
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public StatusAlteracao? Status { get; set; }
    public SistemaAlteracao? Sistema { get; set; }
    public TipoAlteracao? Tipo { get; set; }
    public string? Search { get; set; }
    public string? OrderBy { get; set; }

    public ReportFilterDto() { }
}
