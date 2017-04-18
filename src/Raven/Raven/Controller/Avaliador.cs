using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Controller
{
    public class Avaliador
    {
        /* ################
         * # PROPRIEDADES # 
         * ################ */
        public Aplicador App { get; private set; }
        public int Percentil { get; private set; }

        /* ##############
         * # CONSTRUTOR # 
         * ############## */
        public Avaliador(Aplicador app)
        {
            this.App = app;
            App.CalcularResultado();
            Percentil = CalcularPercentil();
        }

        /* ###########
         * # MÉTODOS # 
         * ########### */
        public int CalcularPercentil()
        {
            int percentil = 0;
            string[][] tabela = App.ExtrairTabela("percentil");
            int noAcertos = App.NoRespostasCorretas;
            int idade = App.Idade;
            var faixa = fft(tabela
                .ElementAt(0)
                .Where(it => it.Length > 0)
                .Select(it => it.Split(' ').Select(int.Parse).ToArray())
                .Select(it => (idade >= it[0]) && (idade <= it[1]))
                .ToArray());
            var pontuacao = tabela
                .Skip(1)
                .Select(stuff => stuff.Skip(1).Select(int.Parse).ToArray())
                .ToArray();
            var percentis = tabela
                .Skip(1)
                .Select(stuff => stuff[0])
                .Select(int.Parse)
                .ToArray();
            var teto = pontuacao[0][faixa];




            return percentil;
        }

        public int fft(bool[] s)
        {
            for (int i = 0; i < s.Count(); i++) if (s[i]) return i;
            return -1;
        }

        public 
    }
}
