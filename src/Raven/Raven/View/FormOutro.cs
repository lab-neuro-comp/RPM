using Raven.Controller;
using System;
using System.Windows.Forms;

namespace Raven.View
{
    public partial class FormOutro : Form
    {
        private Form Mother { get; set; }
        private Aplicador App { get; set; }

        public FormOutro(Form mother, Aplicador app)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            App = app;
            Mother = mother;
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            FormResultado form = new FormResultado(Mother, App);
            form.Show();
            Mother = null;
            Close();
        }
    }
}
