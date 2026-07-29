using ControleTeste.Enums;
using System.ComponentModel.DataAnnotations;

namespace ControleTeste.DTOs;

public record RequisicaoAlteracaoStatusDto(
    [property: Required]
    StatusAlteracao Status,
    string? Observacao
);
