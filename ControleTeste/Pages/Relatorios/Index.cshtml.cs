using ControleTeste.DTOs;
using ControleTeste.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClosedXML.Excel;

namespace ControleTeste.Pages.Relatorios;

public class IndexModel : PageModel
{
    private readonly ControleTeste.Services.IAlteracaoService _service;
    private const int MaxExportRows = 100000;

    public IndexModel(ControleTeste.Services.IAlteracaoService service)
    {
        _service = service;
        Filter = new ReportFilterDto();
    }

    [BindProperty(SupportsGet = true)]
    public ReportFilterDto Filter { get; set; }

    public IEnumerable<ReportRowDto> Rows { get; set; } = Enumerable.Empty<ReportRowDto>();

    public async Task OnGetAsync(int pageNumber = 1, int pageSize = 25)
    {
        var paged = await _service.GetReportAsync(pageNumber, pageSize, Filter);
        Rows = paged.Items;
    }

    public async Task<IActionResult> OnGetExportCsvAsync()
    {
        var rows = await _service.GetReportRowsAsync(Filter, MaxExportRows);

        var ms = new MemoryStream();
        using (var writer = new StreamWriter(ms, System.Text.Encoding.UTF8, 1024, true))
        {
            // header
            writer.WriteLine("AlteracaoId,NumeroAlteracao,Titulo,Descricao,MenuSistema,Tipo,Status,Sistema,DataAbertura,Observacao");
            foreach (var r in rows)
            {
                var line = string.Format("{0},{1},\"{2}\",\"{3}\",\"{4}\",{5},{6},{7},{8},\"{9}\"",
                    r.AlteracaoId,
                    r.NumeroAlteracao,
                    r.Titulo?.Replace("\"", "\"\"") ?? string.Empty,
                    r.Descricao?.Replace("\"", "\"\"") ?? string.Empty,
                    r.MenuSistema?.Replace("\"", "\"\"") ?? string.Empty,
                    (int)r.Tipo,
                    (int)r.Status,
                    (int)r.Sistema,
                    r.DataAbertura.ToString("dd/MM/yyyy HH:mm"),
                    r.Observacao?.Replace("\"", "\"\"") ?? string.Empty
                );
                writer.WriteLine(line);
            }
        }
        ms.Position = 0;
        return File(ms, "text/csv", $"relatorio_alteracoes_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
    }

    public async Task<IActionResult> OnGetExportExcelAsync()
    {
        var rows = await _service.GetReportRowsAsync(Filter, MaxExportRows);

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Relatorio");
        ws.Cell(1, 1).Value = "AlteracaoId";
        ws.Cell(1, 2).Value = "NumeroAlteracao";
        ws.Cell(1, 3).Value = "Titulo";
        ws.Cell(1, 4).Value = "Descricao";
        ws.Cell(1, 5).Value = "MenuSistema";
        ws.Cell(1, 6).Value = "Tipo";
        ws.Cell(1, 7).Value = "Status";
        ws.Cell(1, 8).Value = "Sistema";
        ws.Cell(1, 9).Value = "DataAbertura";
        ws.Cell(1, 10).Value = "Observacao";

        var row = 2;
        foreach (var r in rows)
        {
            ws.Cell(row, 1).Value = r.AlteracaoId;
            ws.Cell(row, 2).Value = r.NumeroAlteracao;
            ws.Cell(row, 3).Value = r.Titulo;
            ws.Cell(row, 4).Value = r.Descricao;
            ws.Cell(row, 5).Value = r.MenuSistema;
            ws.Cell(row, 6).Value = r.Tipo.ToString();
            ws.Cell(row, 7).Value = r.Status.ToString();
            ws.Cell(row, 8).Value = r.Sistema.ToString();
            ws.Cell(row, 9).Value = r.DataAbertura.ToString("dd/MM/yyyy HH:mm");
            ws.Cell(row, 10).Value = r.Observacao;
            row++;
        }

        var ms = new MemoryStream();
        wb.SaveAs(ms);
        ms.Position = 0;
        return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"relatorio_alteracoes_{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx");
    }
}
