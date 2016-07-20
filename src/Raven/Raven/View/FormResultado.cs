using System;
using System.Windows.Forms;

namespace Raven.View
{
    public partial class FormResultado : Form
    {
        public FormResultado(string resultado)
        {
            /* Não se esqueça de que o os labels de baixo estão invisíveis! */
            InitializeComponent(); 
            WindowState = FormWindowState.Maximized;
            var stuff = resultado.Split(' ');
            lblLevel.Text = stuff[0];
            lblNoRespostasCorretas.Text = stuff[1];
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            FormIntro f1 = new FormIntro();
            f1.Show();
            Close();
        }
        
    }
}
