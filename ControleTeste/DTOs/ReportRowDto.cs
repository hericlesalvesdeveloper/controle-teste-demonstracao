using ControleTeste.Enums;

namespace ControleTeste.DTOs;

public record LinhaRelatorioDto(
    int AlteracaoId,
    int NumeroAlteracao,
    string Titulo,
    string Descricao,
    string MenuSistema,
    TipoAlteracao Tipo,
    StatusAlteracao Status,
    SistemaAlteracao Sistema,
    DateTime DataAbertura,
    string Observacao
);
