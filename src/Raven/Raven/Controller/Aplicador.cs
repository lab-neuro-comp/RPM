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
        public List<double> Tempos { get; private set; }
        public List<int> Respostas { get; private set; }
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
            string arquivoPadrao = CamadaAcessoDados.GerarPadraoPeloTeste(NomeTeste);
            string dadosPuros = CamadaAcessoDados.Tudo(arquivoPadrao);
            string[][] percentis = Preparador.ExtrairTabela(dadosPuros, "percentile");
            string[][] validades = Preparador.ExtrairTabela(dadosPuros, "validity");
            var relacaoValidades = ChecarValidade(validades, percentis);
             
            Validade = (relacaoValidades.ContainsValue("INVÁLIDO") || relacaoValidades.Count == 0)?
                "INVÁLIDO" :
                "VÁLIDO";

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
            // TODO Atualizar o formatador para incluir a validade de cada série
            string[] linhasTabela = Formatador.GerarTabela(this);
            CamadaAcessoDados.Salvar(CamadaAcessoDados.GerarResultado(NomeSujeito), linhasTabela);
        }

        /// <summary>
        /// Confere se a execução de um teste foi válida ou não
        /// </summary>
        /// <returns>Para cada tag que identifica a série, é relacionado a string "VÁLIDO" se 
        /// a execução foi válida; "INVÁLIDO" ou "IDADE INVÁLIDA" caso contrário</returns>
        public Dictionary<string, string> ChecarValidade(string[][] validadesPuras, string[][] percentisPuros)
        {
            Dictionary<string, string> saida = new Dictionary<string, string>();
            var esperado = new Dictionary<string, int>(Infra.ParamExtractor.GetExpectedForEachSeries(validadesPuras, NoRespostasCorretas));
            var coletado = new Dictionary<string, int>(Infra.Calculator.RelateSeriesAndAnswers(Series,
                                                                                               Respostas.Zip(OpcoesCorretas, (given, expected) => given == expected)
                                                                                                        .ToArray()));

            // TODO Levar idade em consideração

            foreach (var serie in esperado.Keys.ToArray())
            {
                int discrepancia = Math.Abs(esperado[serie] - coletado[serie]);
                Console.WriteLine($"APLICADOR: |{esperado[serie]} - {coletado[serie]}| = {discrepancia}");
                saida[serie] = (discrepancia <= 2) ? "VÁLIDO" : "INVÁLIDO";
            }

            return saida;
        }
    }
}
