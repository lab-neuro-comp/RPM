using System;
using System.Collections.Generic;
using Raven.Model;
using System.Linq;
using Infra;
using System.Globalization;
using System.Diagnostics;

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
        public int TamanhoDoTeste { get; private set; }
        public int Percentil { get; private set; }
        public Stopwatch Cronometro { get; private set; }

        public Aplicador(string nomeSujeito, string nomeTeste, int idade)
        {
            this.NomeSujeito = nomeSujeito;
            this.NomeTeste = nomeTeste;
            this.Idade = idade;
            this.Cronometro = new Stopwatch();
        }

        public string[] CarregarImagens(int rodada)
        {
            return CamadaAcessoDados.CarregarImagens(NomeTeste, Imagens[rodada], NoOpcoes[rodada]);
        }

        public int ObterTamanhoDoTeste()
        {
            this.TamanhoDoTeste = this.Imagens.Length;
            return TamanhoDoTeste;
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

        public string[] Apresentar(int rodada)
        {
            // Lidando com cronômetro
            if (Cronometro.ElapsedMilliseconds > 0)
            {
                Cronometro.Reset();
            }
            Cronometro.Start();

            // Lidando com a imagem
            return CarregarImagens(rodada);
        }

        public void OuvirResposta(int rodada, int resposta)
        {
            // Registrando cronômetro
            Cronometro.Stop();
            Tempos.Add(Cronometro.ElapsedMilliseconds);

            // Registrando resposta
            Respostas.Add(resposta);
            if (OpcoesCorretas[rodada] == resposta)
            {
                NoRespostasCorretas++;
            }
            
        }

        public string CalcularResultado()
        {
            // preparando dados para cálculo
            string arquivoPadrao = CamadaAcessoDados.GerarPadraoPeloTeste(NomeTeste);
            string dadosPuros = CamadaAcessoDados.Tudo(arquivoPadrao);
            string[][] percentis = Preparador.ExtrairTabela(dadosPuros, "percentile");
            string[][] validades = Preparador.ExtrairTabela(dadosPuros, "validity");

            // calculando resultado
            Percentil = Infra.Calculator.CalculateResult(percentis, NoRespostasCorretas, Idade);
            Validade = ChecarValidade(validades, percentis);
            return $"{Percentil}\t{NoRespostasCorretas}\t{Validade}";
        }

        public void RegistrarCronometro()
        {
            var tempos = Tempos.Select((it) => it.ToString())
                               .ToArray();
            var resultado = this.CalcularResultado().Split('\t');

            CamadaAcessoDados.Salvar(CamadaAcessoDados.GerarResultado(NomeSujeito), 
                                     Infra.Formatter.Format(Idade, 
                                                            resultado[0],
                                                            resultado[1],
                                                            MomentoInicial,
                                                            OpcoesCorretas.Select((it) => it.ToString())
                                                                          .ToArray(),
                                                            Respostas.Select((it) => it.ToString())
                                                                     .ToArray(),
                                                            tempos,
                                                            Validade));
        }

        public Dictionary<string, int> RelacionarSeriesERespostas()
        {
            var x = Infra.ParamExtractor.RelateSeriesAndAnswers(Series,
                                                                OpcoesCorretas,
                                                                Respostas.ToArray());
            return new Dictionary<string, int>(x);
        }

        /// <summary>
        /// Confere se a execução de um teste foi válida ou não
        /// </summary>
        /// <returns>"VÁLIDO" se a execução foi válida; "INVÁLIDO" caso contrário</returns>
        private string ChecarValidade(string[][] validadesPuras, string[][] percentisPuros)
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
            int notaMaxima = Infra.ParamExtractor.GetTopResult(validadesPuras);
            int notaMinima = Infra.ParamExtractor.GetFloorResult(validadesPuras);
            int idadeMinima = Infra.ParamExtractor.GetMinimumAge(percentisPuros);
            int idadeMaxima = Infra.ParamExtractor.GetMaximumAge(percentisPuros); ;
            string saida = "INVÁLIDO";

            // Checando caso base
            if ((NoRespostasCorretas < notaMinima) || (NoRespostasCorretas > notaMaxima))
                return saida;

            // TODO Extrair idades mínima e máxima
            if ((Idade < idadeMinima) || (Idade > idadeMaxima))
                return "IDADE INVÁLIDA";

            // Construindo respostas por série
            respostasPorSerie = RelacionarSeriesERespostas();

            // Construindo respostas esperadas
            var series = Infra.ParamExtractor.GetSeriesList(validadesPuras);
            var indice = NoRespostasCorretas - notaMinima + 1;
            var validades = validadesPuras[indice];
            var valido = true;
            for (int j = 0; (j < series.Count()) && valido; ++j)
            {
                var esperado = -10;
                var coletado = respostasPorSerie[series.ElementAt(j)];
                int.TryParse(validades[j+1], out esperado);
                if (esperado < 0) throw new Exception();
                valido &= (Math.Abs(coletado - esperado) <= 2);
            }
            saida = (valido) ? "VÁLIDO" : "INVÁLIDO";

            return saida;
        }
    }
}
