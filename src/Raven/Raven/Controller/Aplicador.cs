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

        /// <summary>
        /// Carrega as imagens da rodada fornecida.
        /// </summary>
        public string[] CarregarImagens(int rodada)
        {
            return CamadaAcessoDados.CarregarImagens(NomeTeste, Imagens[rodada], NoOpcoes[rodada]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ObterTamanhoDoTeste()
        {
            this.TamanhoDoTeste = this.Imagens.Length;
            return TamanhoDoTeste;
        }

        /// <summary>
        /// Carrega as informações necessárias para a execução do teste.
        /// </summary>
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

        /// <summary>
        /// Mostra os dados relacionados à rodada atual.
        /// </summary>
        /// <param name="rodada"></param>
        /// <returns>As imagens que deverão ser apresentadas nesta rodada.</returns>
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

        /// <summary>
        /// Adiciona a resposta da rodada dada.
        /// </summary>
        /// <param name="rodada">Número da rodada atual, 0 indexado.</param>
        /// <param name="resposta">A resposta dada pelo sujeito. Este método 
        /// não checa se esse valor é válido.</param>
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

        /// <summary>
        /// Calcula o resultado da execução de um teste completo.
        /// </summary>
        /// <returns>Uma string com 3 campos delimitados por tabulação: percentil,
        /// número de respostas corretas e validade do teste, respectivamente.</returns>
        public string CalcularResultado()
        {
            // preparando dados para cálculo
            string arquivoPadrao = CamadaAcessoDados.GerarPadraoPeloTeste(NomeTeste);
            string dadosPuros = CamadaAcessoDados.Tudo(arquivoPadrao);
            string[][] percentis = Preparador.ExtrairTabela(dadosPuros, "percentile");
            string[][] validades = Preparador.ExtrairTabela(dadosPuros, "validity");

            // calculando resultado
            Validade = ChecarValidade(validades, percentis);
            if (Validade == "VÁLIDO")
            {
                Percentil = Infra.Calculator.CalculateResult(percentis, NoRespostasCorretas, Idade);
            }
            
            return $"{Percentil}\t{NoRespostasCorretas}\t{Validade}";
        }

        /// <summary>
        /// Salva na memória a perfomance obtida pelo sujeito.
        /// </summary>
        public void RegistrarCronometro()
        {
            // Preparando variáveis auxiliares
            var tempos = Tempos.Select((it) => it.ToString()).ToArray();
            var resultado = this.CalcularResultado().Split('\t');

            // Construindo colunas com itens repetidos
            var dados = new Dictionary<string, string[]>();
            int limite = OpcoesCorretas.Length;
            var name = new string[limite];
            var age = new string[limite];
            var initial = new string[limite];
            var percentile = new string[limite];
            var correct = new string[limite];
            var incorrect = new string[limite];
            var validity = new string[limite];

            for (int i = 0; i < limite; ++i)
            {
                name[i] = NomeSujeito;
                age[i] = Idade.ToString();
                percentile[i] = Percentil.ToString();
                correct[i] = NoRespostasCorretas.ToString();
                incorrect[i] = (Respostas.Count - NoRespostasCorretas).ToString();
                validity[i] = Validade;
                initial[i] = MomentoInicial;
            }
            
            // Preparando colunas
            dados["name"] = name;
            dados["age"] = age;
            dados["initial"] = initial;
            dados["percentile"] = percentile;
            dados["correct"] = correct;
            dados["incorrect"] = incorrect;
            dados["expected"] = OpcoesCorretas.Select((it) => it.ToString()).ToArray();
            dados["answers"] = Respostas.Select((it) => it.ToString()).ToArray();
            dados["times"] = tempos;
            dados["validity"] = validity;

            // Salvando dados na memória
            string titulo = "Nome;Idade;Momento Inicial;Percentil;# Respostas Corretas;# Respostas Incorretas;Resposta esperada;Resposta dada;Tempos;Validez";
            CamadaAcessoDados.Salvar(CamadaAcessoDados.GerarResultado(NomeSujeito), 
                                     Infra.Formatter.Format(titulo, dados, limite));
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
        /// <returns>"VÁLIDO" se a execução foi válida; "INVÁLIDO" ou "IDADE INVÁLIDA" caso contrário</returns>
        private string ChecarValidade(string[][] validadesPuras, string[][] percentisPuros)
        {
            Dictionary<string, int> respostasPorSerie = new Dictionary<string, int>();
            int notaMaxima = Infra.ParamExtractor.GetTopResult(validadesPuras);
            int notaMinima = Infra.ParamExtractor.GetFloorResult(validadesPuras);
            int idadeMinima = Infra.ParamExtractor.GetMinimumAge(percentisPuros);
            int idadeMaxima = Infra.ParamExtractor.GetMaximumAge(percentisPuros);
            string saida = "INVÁLIDO";

            // Checando caso base
            if ((NoRespostasCorretas < notaMinima) || (NoRespostasCorretas > notaMaxima))
                return saida;
            if ((Idade < idadeMinima) || (Idade > idadeMaxima))
                return "IDADE INVÁLIDA";

            // Construindo respostas por série
            var temp = Infra.ParamExtractor.RelateSeriesAndAnswers(Series,
                                                                   OpcoesCorretas,
                                                                   Respostas.ToArray());
            respostasPorSerie = new Dictionary<string, int>(temp);

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
