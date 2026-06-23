using ControleTeste.Enums;
using System.ComponentModel.DataAnnotations;

namespace ControleTeste.DTOs;

public record RequisicaoAlteracaoDto(
    [Required]
    string NumeroAlteracao,

    [Required]
    [StringLength(50, MinimumLength = 3)]
    string Titulo,

    [StringLength(2000)]
    string Descricao,

    TipoAlteracao Tipo,

    StatusAlteracao Status,

    SistemaAlteracao Sistema,
    string? MenuSistema,

    DateTime DataAbertura,

    string? Observacao
){}
