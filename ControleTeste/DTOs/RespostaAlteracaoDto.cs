using ControleTeste.Enums;

namespace ControleTeste.DTOs;

public record RespostaAlteracaoDto(
    int AlteracaoId,
    string NumeroAlteracao,
    string Titulo,
    string Descricao,
    TipoAlteracao Tipo,
    StatusAlteracao Status,
    SistemaAlteracao Sistema,
    string MenuSistema,
    DateTime DataAbertura,
    string? Observacao
)
{
}
