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
            Percentil = Infra.Calculator.CalculateResult(App.ExtrairTabela("percentile"), App.NoRespostasCorretas, App.Idade);
            // TODO Calcular percentil usando uma lógica própria
        }

        /* ###########
         * # MÉTODOS # 
         * ########### */
    }
}
