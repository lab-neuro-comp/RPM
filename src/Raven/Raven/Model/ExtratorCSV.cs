using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Model
{
    public class ExtratorCSV
    {
        public string NomeTeste { get; private set; }
        public ArrayList Idades { get; private set; }
        public ArrayList Pontos { get; private set; }
        public ArrayList Percentis { get; private set; }

        public ExtratorCSV()
        {
            this.Idades = new ArrayList();
            this.Pontos = new ArrayList();
            this.Percentis = new ArrayList();
        }

        public ExtratorCSV(string nomeTeste) : this()
        {
            this.NomeTeste = nomeTeste;
            this.Carregar();
        }

        public void Carregar()
        {
            StreamReader csv = new StreamReader(NomeTeste);

            PopularRelacionamento(csv.ReadLine());
            for (string line = csv.ReadLine(); line != null; line = csv.ReadLine())
            {
                string[] fields = line.Split(',');
                Percentis.Add(int.Parse(fields[0]));
                for (int i = 1; i < fields.Length; ++i)
                {
                    ArrayList lista = Pontos[i - 1] as ArrayList;
                    lista.Add(int.Parse(fields[i]));
                }                    
            }

            csv.Close();
        }

        private void PopularRelacionamento(string header)
        {
            foreach (string field in header.Split(','))
            {
                try
                {
                    string[] data = field.Split(' ');
                    int minimum = int.Parse(data[0]);
                    int maximum = int.Parse(data[1]);
                    int[] range = { minimum, maximum };
                    Idades.Add(range);
                    Pontos.Add(new ArrayList());
                }
                catch (Exception any)
                {
                    continue;
                }
            }
        }

        internal int Relacionar(int idade, int noRespostasCorretas)
        {
            int resultado = 0;
            int indice = 0;

            for (indice = 0; indice < Idades.Capacity; indice++)
            {
                int[] idades = Idades[indice] as int[];
                if (idade >= idades[0] && idade <= idades[1])
                {
                    break;
                }
            }

            ArrayList pontos = Pontos[indice] as ArrayList;
            if (noRespostasCorretas >= (int) pontos[0])
            {
                resultado = 0;
            }
            else for (resultado = 1; resultado < Pontos.Capacity; resultado++)
            {
                if (noRespostasCorretas >= (int) pontos[resultado] && 
                    noRespostasCorretas < (int) pontos[resultado - 1])
                {
                    break;
                }
            }

            return (int) Percentis[resultado];
        }
    }
}
