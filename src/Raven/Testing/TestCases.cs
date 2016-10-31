using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Controller;
using System.Diagnostics;

namespace Testing
{
    class TestCases
    {
        static Aplicador App;

        static void Main(string[] args)
        {
            Console.WriteLine("--- # Testing 'cor' test");
            App = new Aplicador("lil one", "cor", 9);
            Stopwatch clock = new Stopwatch();
            App.PrepararTeste();
            Console.WriteLine("Momento inicial: " + App.MomentoInicial);
            Console.WriteLine("Respostas dadas:");
            for (var i = 0; i < App.ObterTamanhoDoTeste(); ++i)
            {
                clock.Start();
                //var resposta = GenerateAnswersForColorful()[i];
                var resposta = GenerateInvalidAnswers(App.TamanhoDoTeste)[i];
                Console.WriteLine("- " + resposta);
                App.OuvirResposta(i, resposta);
                clock.Stop();
                App.OuvirDuracao(i, clock.ElapsedMilliseconds);
                clock.Reset();

            }
            Console.WriteLine("# Respostas corretas: " + App.NoRespostasCorretas);
            Console.WriteLine("--- # Assessing results");
            Console.WriteLine($"Resultado calculado:\n\t{App.CalcularResultado()}");
            Console.WriteLine("...");
            Console.ReadLine();
        }

        static int[] GenerateAnswersForColorful()
        {
            return new int[] { 4, 5, 1, 2, 6, 3, 6, 2, 3, 4, 6, 5,
                               4, 5, 1, 6, 2, 1, 3, 3, 6, 4, 5, 5,
                               2, 6, 1, 2, 2, 1, 5 ,6 ,6 ,3, 4, 5, 5};
        }

        static int[] GenerateInvalidAnswers(int size)
        {
            List<int> answers = new List<int>();

            for (int i = 0; i < size; ++i)
            {
                answers.Add((i % 6) + 1);
            }

            return answers.ToArray();
        }
    }
}
