using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Raven.Model
{
    public class CamadaAcessoDados
    {
        public static string CaminhoAtual = @".\assets\";
        public static string CaminhoResultados = @".\results\";
        public static string CaminhoConfig = @"config\versions.txt";
        public static string CaminhoDados = @"config\";

        public static string Tudo(string entrada)
        {
            return File.ReadAllText(entrada);
        }

        public static string[] CadaLinha(string entrada)
        {
            StreamReader arquivo = new StreamReader(entrada);
            List<string> caixa = new List<string>();

            for (var linha = arquivo.ReadLine(); linha != null; linha = arquivo.ReadLine())
            {
                caixa.Add(linha);
            }

            return caixa.ToArray<string>();
        }

        public static string GerarParametrosPeloTeste(string teste)
        {
            return CaminhoAtual + CaminhoDados + teste + ".txt";
        }

        public static string GerarPadraoPeloTeste(string teste)
        {
            return CaminhoAtual + CaminhoDados + teste + ".xml";
        }

        public static string GerarArquivoDeInstrucoes()
        {
            return CaminhoAtual + CaminhoDados + "instructions.txt";
        }

        public static bool ChecarExistenciaDoArquivo(string arquivo)
        {
            return File.Exists(arquivo);
        }

        public static string GerarResultado(string sujeito)
        {
            return CaminhoResultados + sujeito + ".csv";
        }

        public static string[] CarregarImagens(string test, string img, int noImgs)
        {
            string[] imgs = new string[noImgs + 1];

            if (img.Length <= 2)
            {
                return null;
            }
            else
                for (int i = 0; i <= noImgs; ++i)
                {
                    imgs[i] = CaminhoAtual + test + @"\" + img + "." + i + ".png";
                }

            return imgs;
        }

        public static void Salvar(string onde, string[] linhas)
        {
            File.WriteAllLines(onde, linhas);
        }
    }
}
