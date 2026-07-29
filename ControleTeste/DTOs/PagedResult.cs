namespace ControleTeste.DTOs;

public record ResultadoPaginado<T>(IEnumerable<T> Itens, int TotalItens, int NumeroPagina, int TamanhoPagina)
{
}
