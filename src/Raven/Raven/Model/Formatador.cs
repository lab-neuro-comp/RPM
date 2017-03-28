using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Controller;

namespace Raven.Model
{
    public class Formatador
    {
        public static string[] GerarTabela(Aplicador app)
        {
            // Preparando variáveis auxiliares
            var tempos = app.Tempos.Select((it) => it.ToString()).ToArray();
            var resultado = app.CalcularResultado().Split('\t');

            // Construindo colunas com itens repetidos
            var dados = new Dictionary<string, string[]>();
            int limite = app.OpcoesCorretas.Length;
            var name = new string[limite];
            var age = new string[limite];
            var initial = new string[limite];
            var percentile = new string[limite];
            var correct = new string[limite];
            var incorrect = new string[limite];
            var validity = new string[limite];

            for (int i = 0; i < limite; ++i)
            {
                name[i] = app.NomeSujeito;
                age[i] = app.Idade.ToString();
                percentile[i] = app.Percentil.ToString();
                correct[i] = app.NoRespostasCorretas.ToString();
                incorrect[i] = (app.Respostas.Count - app.NoRespostasCorretas).ToString();
                validity[i] = app.Validade;
                initial[i] = app.MomentoInicial;
            }

            // Preparando colunas
            dados["name"] = name;
            dados["age"] = age;
            dados["initial"] = initial;
            dados["percentile"] = percentile;
            dados["correct"] = correct;
            dados["incorrect"] = incorrect;
            dados["expected"] = app.OpcoesCorretas.Select((it) => it.ToString()).ToArray();
            dados["answers"] = app.Respostas.Select((it) => it.ToString()).ToArray();
            dados["times"] = tempos;
            dados["validity"] = validity;

            // Salvando dados na memória
            string titulo = "Nome;Idade;Momento Inicial;Percentil;# Respostas Corretas;# Respostas Incorretas;Resposta esperada;Resposta dada;Tempos;Validez";
            return Infra.Formatter.Format(titulo, dados);
        }
    }
}
