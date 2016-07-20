using System;
using System.Windows.Forms;

namespace Raven.View
{
    public partial class FormResultado : Form
    {
        public FormResultado()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
        }

        public FormResultado(string resultado) : this()
        {
            lblLevel.Text = resultado;
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
