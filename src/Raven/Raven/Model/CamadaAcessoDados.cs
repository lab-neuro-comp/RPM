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
        public static string CaminhoConfig = @"config\versions.txt";

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


    }
}
