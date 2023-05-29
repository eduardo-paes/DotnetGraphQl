using System.ComponentModel;

namespace Domain.Entities.Enums
{
    public enum EProjectStatus
    {
        [Description("Aberto: Projeto aguardando ser completamente preenchido para então ser Submetido.")]
        Opened,

        [Description("Submetido: Projeto Submetido e aguardando avaliação por parte do Avaliador.")]
        Submitted,

        [Description("Avaliação: Projeto Submetido e disponível para avaliação por parte do Avaliador.")]
        Evaluation,

        [Description("Indeferido: Projeto reprovado pelo Avaliador, esse poderá ser Ressubmetido no período de Recurso, sem alteração do projeto.")]
        Rejected,

        [Description("Deferido: Projeto está pronto para receber os documentos necessários para ser iniciado - os documentos devem ser entregues dentro do período estipulado no edital.")]
        Accepted,

        [Description("Análise: Projeto recebeu toda a documentação necessária e essa se encontra em análise pelo Avaliador.")]
        DocumentAnalysis,

        [Description("Iniciado: Projeto recebeu toda a documentação necessária e essa foi aprovada.")]
        Started,

        [Description("Pendente: Projeto recebeu a documentação necessária, porém essa não foi aprovada - os documentos podem ser reenviados dentro do período estipulado no edital.")]
        Pending,

        [Description("Cancelado: Projeto cancelado pelo professor ou administrador.")]
        Cancelled,

        [Description("Encerrado: Projeto concluído, o Orientador possui 30 dias para entregar o Relatório Final, do contrário sua conta será suspensa por XX anos.")]
        Closed
    }
}