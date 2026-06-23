using ControleTeste.Enums;

namespace ControleTeste.Models;

public class Alteracao
{
    public int AlteracaoId { get; private set; }
    public int NumeroAlteracao { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public TipoAlteracao Tipo { get; private set; }
    public StatusAlteracao Status { get; private set; }
    public SistemaAlteracao Sistema { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public string Observacao { get; private set; } = string.Empty;
    public string MenuSistema { get; private set; } = string.Empty;

    public Alteracao(int numeroAlteracao, string titulo, 
        string descricao, TipoAlteracao tipo, StatusAlteracao status, SistemaAlteracao sistema,
        string menuSistema, DateTime dataAbertura, string observacao) {

        NumeroAlteracao = numeroAlteracao;
        Titulo = titulo;
        Descricao = descricao;
        Tipo = tipo;
        Status = status;
        Sistema = sistema;
        MenuSistema = menuSistema;
        DataAbertura = dataAbertura;
        Observacao = observacao;
    }

    public void AlterarStatus(StatusAlteracao status, string? observacao)
    {
        if(status == StatusAlteracao.Retorno && string.IsNullOrEmpty(observacao))
        {
            throw new ControleTeste.Exceptions.AppValidationException("Observação é obrigatória para o status de Retorno.");
        }

        Status = status;
        Observacao = observacao ?? string.Empty;
    }

    // Permite atualizar campos básicos da alteração
    public void AtualizarDados(int numeroAlteracao, string titulo, string descricao, TipoAlteracao tipo, SistemaAlteracao sistema, string? menuSistema)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título é obrigatório.");

        NumeroAlteracao = numeroAlteracao;
        Titulo = titulo;
        Descricao = descricao ?? string.Empty;
        Tipo = tipo;
        Sistema = sistema;
        MenuSistema = menuSistema ?? string.Empty;
    }

    // Atualiza somente observação (pode ser usada sem alterar status)
    public void AtualizarObservacao(string? observacao)
    {
        Observacao = observacao ?? string.Empty;
    }
}