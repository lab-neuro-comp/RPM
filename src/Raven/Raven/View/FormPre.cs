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
        private FormIntro Mother { get; set; }

        public FormPre(FormIntro mother, Aplicador app)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            App = app;
            Mother = mother;
            UpdateInstructions(Preparador.ObterInstrucoes());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormOps form = new FormOps(Mother, App);
            form.Show();
            form.Test();
            this.Mother = null;
            this.Close();
        }
    }
}
