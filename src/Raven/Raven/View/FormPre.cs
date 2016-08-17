using Raven.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Raven.View
{
    public partial class FormPre : Form
    {
        private Aplicador App { get; set; }

        public FormPre(Aplicador app)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            App = app;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormOps form = new FormOps(App);
            form.Show();
            form.Test();
            this.Close();
        }
    }
}
