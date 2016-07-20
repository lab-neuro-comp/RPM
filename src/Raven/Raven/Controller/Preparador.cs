using System;
using System.Collections.Generic;
using System.IO;
using Infra;
using Raven.Model;

namespace Raven.Controller
{
    public class Preparador
    {
        public string[] Caminhos { get; private set; }
        public string[] Testes { get; private set; }
        public string[] ImagensPrincipais { get; private set; }
        public int[] Opcoes { get; private set; }
        public int[] Respostas { get; private set; }

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

        // TODO Adicionar esta parte para a camada de acesso de dados
        public void CarregarOpcoes(string teste)
        {
            string config = CamadaAcessoDados.CaminhoAtual + @"config\" + teste + ".txt";
            string caminhoDados = CamadaAcessoDados.CaminhoAtual + teste + @"\";
            List<string> imgs = new List<string>();
            List<int> ops = new List<int>();
            List<int> ans = new List<int>();
            StreamReader file = new StreamReader(config);

            for (string line = file.ReadLine(); line != null; line = file.ReadLine())
            {
                string[] data = line.Split(' ');
                imgs.Add(data[0]);
                ops.Add(int.Parse(data[1]));
                ans.Add(int.Parse(data[2]));
            }

            ImagensPrincipais = imgs.ToArray();
            Opcoes = ops.ToArray();
            Respostas = ans.ToArray();
        }

        // TODO Adicionar esta parte para o módulo de infraestrutura
        internal int CalcularResultado(string nomeTeste, 
                                       int noRespostasCorretas, 
                                       int idade)
        {
            string csvFileName = CamadaAcessoDados.CaminhoAtual + nomeTeste + ".csv";
            ExtratorCSV extrator = new ExtratorCSV(csvFileName);
            return extrator.Relacionar(idade, noRespostasCorretas);
        }
    }
}
