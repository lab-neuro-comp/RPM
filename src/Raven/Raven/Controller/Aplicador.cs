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

        public int ObterTamanhoDoTeste()
        {
            return this.Imagens.Length;
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
            /*
            # Input
            + Test description, relating questions to series
            + Validity table, relating # of correct answers and expected results in each series
            + List of answers given by the subject

            # Midput
            + Map relating the series and the # of correct answers given by the subject
            + Map relating the series and the expected # of correct answers as prescribed by the subject's # of correct answers

            # Output
            + Test validity
            */

            Dictionary<string, int> respostasPorSerie = new Dictionary<string, int>();
            Dictionary<string, int> repostasEsperadas = null;
            int notaMaxima = Infra.ParamExtractor.GetTopResult(validadesPuras);
            int notaMinima = Infra.ParamExtractor.GetFloorResult(validadesPuras);
            string saida = "INVÁLIDO";
            int tamanhoDoTeste = Respostas.Count;

            // Checando caso base
            if ((NoRespostasCorretas < notaMinima) || (NoRespostasCorretas > notaMaxima))
                return saida;

            // Construindo respostas por série
            for (int i = 0; i < tamanhoDoTeste; ++i)
            {
                var serie = Series[i];
                var contagem = 0;
                var correto = Respostas.ElementAt(i) == OpcoesCorretas[i];

                respostasPorSerie.TryGetValue(serie, out contagem);
                respostasPorSerie.Add(serie, contagem + ((correto)? 1 : 0));
            }

            // Construindo respostas esperadas
            var series = validadesPuras[0].Where((it) => it.Length > 0);
            var indice = NoRespostasCorretas - notaMinima + 1;
            var linha = validadesPuras[indice];

            // Checando se está certo
            saida = $"{NoRespostasCorretas} {linha[0]}";
            return saida;
        }
    }
}
