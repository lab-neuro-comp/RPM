using System;
using System.Collections.Generic;
using Raven.Model;
using System.Linq;
using Infra;

namespace Raven.Controller
{
    public class Aplicador
    {
        public string NomeTeste { get; private set; }
        public int Idade { get; private set; }
        public string[] Imagens { get; private set; }
        public int NoRespostasCorretas { get; private set; }
        private List<double> Tempos { get; set; }
        public int[] NoOpcoes { get; private set; }
        public int[] OpcoesCorretas { get; private set; }

        public Aplicador(string nomeTeste, int idade)
        {
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

            // Preparando parâmetros do teste
            string parametros = CamadaAcessoDados.GerarParametrosPeloTeste(NomeTeste);
            string[] dadosPuros = CamadaAcessoDados.CadaLinha(parametros);
            Imagens = Infra.ParamExtractor.GetImages(dadosPuros);
            NoOpcoes = Infra.ParamExtractor.GetNoOptions(dadosPuros);
            OpcoesCorretas = Infra.ParamExtractor.GetCorrectOptions(dadosPuros);

            // Checando se os dados carregados estão corretos
            //Console.WriteLine(OpcoesCorretas.Aggregate("", (box, it) => $"{box} {it}"));
        }

        public void OuvirResposta(int rodada, int resposta)
        {
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
            string[] dadosPuros = CamadaAcessoDados.CadaLinha(arquivoPadrao);
            string[][] tabela = Infra.ParamExtractor.GenerateTableFromCsv(dadosPuros);

            // calculando resultado
            // TODO Calcular resultado
            int percentil = Infra.Calculator.CalculateResult(tabela, NoRespostasCorretas);
            return $"{percentil} {NoRespostasCorretas} {percentil}";
            //return percentil.ToString();
        }
    }
}
