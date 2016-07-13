using System;
using System.Collections.Generic;

namespace Raven
{
    public class Aplicador
    {
        public string NomeTeste { get; private set; }
        public Preparador DAL { get; private set; }
        public string[] Imagens { get; private set; }
        public int NoRespostasCorretas { get; private set; }
        private List<double> Tempos { get; set; }
        public int Idade { get; private set; }

        public Aplicador()
        {
            DAL = new Preparador();
        }

        public Aplicador(string nomeTeste) : this()
        {
            this.NomeTeste = nomeTeste;
            DAL.CarregarOpcoes(nomeTeste);
            Imagens = DAL.ImagensPrincipais;
            Idade = 21;
        }

        public Aplicador(string nomeTeste, int idade) : this(nomeTeste)
        {
            this.Idade = idade;
        }

        public string[] CarregarImagens(int rodada)
        {
            return DAL.CarregarImagens(NomeTeste, 
                                       Imagens[rodada], 
                                       DAL.Opcoes[rodada]);
        }

        public void PrepararTeste()
        {
            NoRespostasCorretas = 0;
            Tempos = new List<double>();
        }

        public void OuvirResposta(int rodada, int resposta)
        {
            if (DAL.Respostas[rodada] == resposta)
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
            return DAL.CalcularResultado(NomeTeste, NoRespostasCorretas, Idade).ToString();
        }
    }
}
