using ControleTeste.Enums;
using System.ComponentModel.DataAnnotations;

namespace ControleTeste.DTOs;

public record ChangeStatusRequestDto(
    [property: Required]
    StatusAlteracao Status,
    string? Observacao
);
