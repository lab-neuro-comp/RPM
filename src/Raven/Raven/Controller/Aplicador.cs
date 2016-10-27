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

            // Checando se os dados carregados estão corretos
            //Console.WriteLine(OpcoesCorretas.Aggregate("", (box, it) => $"{box} {it}"));

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
            // TODO Extrair dados do arquivo XML
            // preparando dados para cálculo
            string arquivoPadrao = CamadaAcessoDados.GerarPadraoPeloTeste(NomeTeste);
            string[] dadosPuros = CamadaAcessoDados.CadaLinha(arquivoPadrao);
            string[][] tabela = Infra.ParamExtractor.GenerateTableFromCsv(dadosPuros);

            // calculando resultado
            int percentil = Infra.Calculator.CalculateResult(tabela, NoRespostasCorretas, Idade);
            return $"{percentil}\t{NoRespostasCorretas}";
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
                                                            tempos));
        }

    }
}
