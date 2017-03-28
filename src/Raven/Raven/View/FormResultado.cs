using Raven.Controller;
using System;
using System.Windows.Forms;

namespace Raven.View
{
    public partial class FormResultado : Form
    {
        private Form Mother { get; set; }

        public FormResultado(Form mother, Aplicador app)
        {
            Mother = mother;
            /* Não se esqueça de que o os labels de baixo estão invisíveis! */
            string resultado = app.CalcularResultado();
            app.RegistrarCronometro();
            InitializeComponent(); 
            WindowState = FormWindowState.Maximized;
            var stuff = resultado.Split('\t');
            lblLevel.Text = stuff[0];
            lblNoRespostasCorretas.Text = stuff[1];
            lblResultado.Text = stuff[2];
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Mother = null;
            Application.Exit();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Mother.Show();
            Close();
        }
        
    }
}
