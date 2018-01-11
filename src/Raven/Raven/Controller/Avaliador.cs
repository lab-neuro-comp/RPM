using System.Linq;

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
            Percentil = CalcularPercentil();
        }

        /* ###########
         * # MÉTODOS # 
         * ########### */
        /// <summary>
        /// Calcula o percentil obtido pelo sujeito baseado na sua performance no teste e nas tabelas
        /// fornecidas para o seu cálculo pelo aplicador.
        /// </summary>
        /// <returns>O percentil calculado</returns>
        public int CalcularPercentil()
        {
            // TODO Implementar este método em F#
            int percentil = 1;
            string[][] tabela = App.ExtrairTabela("percentile");
            int noAcertos = App.NoRespostasCorretas;
            int idade = App.Idade;
            var faixa = FindFirstTrue(tabela
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
            var limite = pontuacao.Length;

            if (noAcertos >= teto)
            {
                percentil = percentis[0];
            }
            else for (int i = 1; i < limite; i++)
            {
                int chao = pontuacao[i][faixa];
                //Console.WriteLine($"{percentis[i]}. {chao} < {noAcertos} < {teto}");
                if ((chao <= noAcertos) && (teto >= noAcertos))
                {
                    percentil = percentis[i];
                }
                teto = chao;
            }


            return percentil;
        }

        public int FindFirstTrue(bool[] s)
        {
            for (int i = 0; i < s.Count(); i++) if (s[i]) return i;
            return -1;
        }

        // TODO Implementar um método para checar a validade do teste
    }
}
