using System;
using System.Windows.Forms;

namespace Raven
{
    public partial class FormResultado : Form
    {
        public FormResultado()
        {
            InitializeComponent();
        }

        public FormResultado(string resultado, int noRespostasCorretas) : this()
        {
            lblResultado.Text = resultado;
            lblNoRespostasCorretas.Text = noRespostasCorretas.ToString();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            Close();
        }
    }
}
