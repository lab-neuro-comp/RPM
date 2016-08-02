using Raven.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Raven.View
{
    public partial class FormOutro : Form
    {
        private Aplicador App { get; set; }

        public FormOutro(Aplicador app)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            App = app;
        }

        private void buttonFinish_Click(object sender, EventArgs e)
        {
            FormResultado form = new FormResultado(App);
            form.Show();
            Close();
        }
    }
}
