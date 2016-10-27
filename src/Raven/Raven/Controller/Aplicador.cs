using System;
using System.Collections.Generic;
using Raven.Model;
using System.Linq;
using Infra;
using System.Globalization;

namespace Raven.Controller
{
    public class Aplicador
    {
        public string NomeSujeito { get; private set; }
        public string NomeTeste { get; private set; }
        public int Idade { get; private set; }
        public string MomentoInicial { get; private set; }
        public string[] Imagens { get; private set; }
        public int NoRespostasCorretas { get; private set; }
        private List<double> Tempos { get; set; }
        private List<int> Respostas { get; set; }
        public int[] NoOpcoes { get; private set; }
        public int[] OpcoesCorretas { get; private set; }
        public string[] Series { get; private set; }
        public string Validade { get; private set; }

        public Aplicador(string nomeSujeito, string nomeTeste, int idade)
        {
            this.NomeSujeito = nomeSujeito;
            this.NomeTeste = nomeTeste;
            this.Idade = idade;
        }

        public string[] CarregarImagens(int rodada)
        {
            return CamadaAcessoDados.CarregarImagens(NomeTeste, Imagens[rodada], NoOpcoes[rodada]);
        }

        public void PrepararTeste()
        {
            // Preparando resultados do teste
            NoRespostasCorretas = 0;
            Tempos = new List<double>();
            Respostas = new List<int>();

            // Preparando parâmetros do teste
            string parametros = CamadaAcessoDados.GerarParametrosPeloTeste(NomeTeste);
            string[] dadosPuros = CamadaAcessoDados.CadaLinha(parametros);
            Imagens = Infra.ParamExtractor.GetImages(dadosPuros);
            NoOpcoes = Infra.ParamExtractor.GetNoOptions(dadosPuros);
            OpcoesCorretas = Infra.ParamExtractor.GetCorrectOptions(dadosPuros);
            Series = Infra.ParamExtractor.GetSeries(dadosPuros);

            // Checando se os dados carregados estão corretos
            //Console.WriteLine(Series.Aggregate("", (box, it) => $"{box}\n{it}"));

            // Marcando começo do teste
            MomentoInicial = DateTime.Now.ToString(new CultureInfo("pt-BR"));
        }

        public void OuvirResposta(int rodada, int resposta)
        {
            Respostas.Add(resposta);
            if (OpcoesCorretas[rodada] == resposta)
            {
                NoRespostasCorretas++;
            }
        }

        public void OuvirDuracao(int rodada, double tempo)
        {
            Tempos.Add(tempo);
        }

        public string CalcularResultado()
        {
            // preparando dados para cálculo
            string arquivoPadrao = CamadaAcessoDados.GerarPadraoPeloTeste(NomeTeste);
            string dadosPuros = CamadaAcessoDados.Tudo(arquivoPadrao);
            string[][] percentis = Preparador.ExtrairTabela(dadosPuros, "percentile");
            string[][] validades = Preparador.ExtrairTabela(dadosPuros, "validity");

            // calculando resultado
            int percentil = Infra.Calculator.CalculateResult(percentis, NoRespostasCorretas, Idade);
            Validade = ChecarValidade(validades);
            return $"{percentil}\t{NoRespostasCorretas}\t{Validade}";
        }

        public void RegistrarCronometro()
        {
            var tempos = Tempos.Select((it) => it.ToString())
                               .ToArray();
            var resultado = this.CalcularResultado().Split('\t');

            CamadaAcessoDados.Salvar(CamadaAcessoDados.GerarResultado(NomeSujeito), 
                                     Infra.Formatter.Format(resultado[0],
                                                            resultado[1],
                                                            MomentoInicial,
                                                            OpcoesCorretas.Select((it) => it.ToString())
                                                                          .ToArray(),
                                                            Respostas.Select((it) => it.ToString())
                                                                     .ToArray(),
                                                            tempos,
                                                            Validade));
        }

        /// <summary>
        /// Confere se a execução de um teste foi válida ou não
        /// </summary>
        /// <returns>"VÁLIDO" se a execução foi válida; "INVÁLIDO" caso contrário</returns>
        private string ChecarValidade(string[][] validadesPuras)
        {
            Dictionary<string, int[]> validades;

            // Construindo dicionário de validades
            var series = validadesPuras[0].Where(it => it.Length > 0);

            return series.Aggregate((box, it) => $"{box} {it}");
        }
    }
}
