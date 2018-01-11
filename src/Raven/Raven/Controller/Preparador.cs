using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Model;
using System.Xml;

namespace Raven.Controller
{
    public class Preparador
    {
        public string[] Caminhos { get; private set; }
        public string[] Testes { get; private set; }
        public string[] ImagensPrincipais { get; private set; }

        public Preparador()
        {

        }

        public void CarregarTestes()
        {
            string entrada = CamadaAcessoDados.CaminhoAtual + CamadaAcessoDados.CaminhoConfig;
            string[] linhas = CamadaAcessoDados.CadaLinha(entrada);
            List<string> paths = new List<string>();
            List<string> tests = new List<string>();

            foreach (string linha in linhas)
            {
                string[] data = linha.Split(' ');
                paths.Add(data[0]);
                tests.Add(data[1]);
            }

            this.Caminhos = paths.ToArray();
            this.Testes = tests.ToArray();
        }

        public static string[][] ExtrairTabela(string dadoPuro, string tag)
        {
            var xml = new XmlDocument();
            var spec = $"/test/{tag}/text()";
            var linhas = new LinkedList<string[]>();
            XmlNodeList nos;

            xml.LoadXml(dadoPuro);
            nos = xml.SelectNodes(spec);

            foreach (XmlNode no in nos)
            {
                var colunas = no.Value.Split('\n');
                foreach (var coluna in colunas)
                {
                    var linha = coluna.Trim(' ', '\r');
                    if (linha.Length > 0)
                    {
                        linhas.AddLast(linha.Split(';'));
                    }
                }

            }

            return linhas.ToArray();
        }

        public static string ObterInstrucoes()
        {
            string instrucoes = null;
            string arquivoInstrucoes = CamadaAcessoDados.GerarArquivoDeInstrucoes();

            if (CamadaAcessoDados.ChecarExistenciaDoArquivo(arquivoInstrucoes))
            {
                instrucoes = CamadaAcessoDados.Tudo(arquivoInstrucoes);
            }

            Console.WriteLine(CamadaAcessoDados.ChecarExistenciaDoArquivo(arquivoInstrucoes));
            return instrucoes;
        }
    }
}
