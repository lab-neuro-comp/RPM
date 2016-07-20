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
    }
}
