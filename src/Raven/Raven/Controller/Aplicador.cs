﻿using System;
using System.Collections.Generic;
using Raven.Model;
using System.Linq;
using Raven.Model.Calculator;
using System.Globalization;
using System.Diagnostics;

namespace Raven.Controller
{
    /// <summary>
    /// Esta classe representa o controlador central nesta aplicação, fazendo a ponte entre 
    /// as classes que podem acessar a memória; as que foram desenhadas para operar a UI; e
    /// as que realizam os cálculos para obter os resultados.
    /// </summary>
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
        public int Percentil { get; private set; } = 1;
        public Stopwatch Cronometro { get; private set; }

        /// <summary>
        /// Cria uma nova aplicação de teste.
        /// </summary>
        /// <param name="nomeSujeito">Nome do sujeito</param>
        /// <param name="nomeTeste">Código de identificação do teste</param>
        /// <param name="idade">Idade do sujeito</param>
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
        /// Calcula o tamanho do teste baseado no número de imagens
        /// </summary>
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
            Imagens = Calculator.GetImages(dadosPuros);
            NoOpcoes = Calculator.GetNoOptions(dadosPuros);
            OpcoesCorretas = Calculator.GetCorrectOptions(dadosPuros);
            Series = Calculator.GetSeries(dadosPuros);

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
            string[][] percentis = ExtrairTabela("percentile");
            string[][] validades = ExtrairTabela("validity");
            var relacaoValidades = ChecarValidade(validades, percentis);
            var avaliador = new Avaliador(this);

            // TODO Definir entradas e saídas do aplicador
            Percentil = avaliador.Percentil;
            Validade = (relacaoValidades.ContainsValue("INVÁLIDO") || relacaoValidades.Count == 0)?
                "INVÁLIDO" :
                "VÁLIDO";
            
            return $"{Percentil}\t{NoRespostasCorretas}\t{Validade}"; 
        }

        /// <summary>
        /// Calcula os percentil relacionado à execução do teste atual. O teste já deve ter sido terminado para
        /// este método devolver um resultado válido. Este método está deprecado! Prefira a classe "Avaliador"
        /// </summary>
        /// <param name="percentis">A tabela de percentis como dada pelas especificações</param>
        /// <returns>O percentil baseado na execução atual do teste.</returns>
        public int CalcularPercentil(string[][] percentis)
        {
            return Calculator.CalculateResult(percentis, NoRespostasCorretas, Idade);
        }

        /// <summary>
        /// Extrai a dada tabela do arquivo de dados padrão
        /// </summary>
        public string[][] ExtrairTabela(string qual)
        {
            return ExtrairTabela(NomeTeste, qual);
        }

        /// <summary>
        /// Extrai a dada tabela do arquivo de dados padrão de um teste.
        /// </summary>
        /// <param name="teste">código do teste a ser utilizado de parâmetro.</param>
        /// <param name="qual">código da tabela que deverá ser extraída.</param>
        /// <returns>a tabela desejada</returns>
        public static string[][] ExtrairTabela(string teste, string qual)
        {
            return Preparador.ExtrairTabela(CamadaAcessoDados.Tudo(CamadaAcessoDados.GerarPadraoPeloTeste(teste)), qual);
        }

        /// <summary>
        /// Salva na memória a perfomance obtida pelo sujeito.
        /// </summary>
        public void RegistrarCronometro()
        {
            // TODO Atualizar o formatador para incluir a validade de cada série
            string[] linhasTabela = Raven.Model.Formatador.GerarTabela(this);
            CamadaAcessoDados.Salvar(CamadaAcessoDados.GerarResultado(NomeSujeito), linhasTabela);
        }

        /// <summary>
        /// Gera a tabela de saida para ser salva no arquivo desejado.
        /// </summary>
        /// <returns>Uma string onde cada linha é um dado relativo a uma etapa da execução do teste</returns>
        public string ObterCronometro()
        {
            return Raven.Model.Formatador.GerarTabela(this).Aggregate("", (box, it) => $"{box}{it}\n");
        }

        /// <summary>
        /// Confere se a execução de um teste foi válida ou não
        /// </summary>
        /// <returns>Para cada tag que identifica a série, é relacionado a string "VÁLIDO" se 
        /// a execução foi válida; "INVÁLIDO" ou "IDADE INVÁLIDA" caso contrário</returns>
        public Dictionary<string, string> ChecarValidade(string[][] validadesPuras, string[][] percentisPuros)
        {
            Dictionary<string, string> saida = new Dictionary<string, string>();
            var esperado = new Dictionary<string, int>(Calculator.GetExpectedForEachSeries(validadesPuras, NoRespostasCorretas));
            var coletado = new Dictionary<string, int>(Calculator.RelateSeriesAndAnswers(Series,
                                                                                         Respostas.Zip(OpcoesCorretas, (given, expected) => given == expected)
                                                                                                  .ToArray()));

            // TODO Levar idade em consideração
            foreach (var serie in coletado.Keys)
            {
                if (esperado.ContainsKey(serie))
                {
                    int discrepancia = Math.Abs(esperado[serie] - coletado[serie]);
                    saida[serie] = (discrepancia <= 2) ? "VÁLIDO" : "INVÁLIDO";
                }
                else
                {
                    saida[serie] = "INVÁLIDO";
                }
            }

            return saida;
        }
    }
}
