using System;
using System.Windows.Forms;
using Raven.Controller;

namespace Raven.View
{
    public partial class FormIntro : Form
    {
        private Preparador Cook { get; set; }

        public FormIntro()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            PopulateItems();
        }

        private void PopulateItems()
        {
            Cook = new Preparador();
            Cook.CarregarTestes();
            foreach(string item in Cook.Testes)
                comboOps.Items.Add(item);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idade;

            try
            {
                idade = int.Parse(textIdade.Text);
            }
            catch (Exception any)
            {
                return;
            }
                
            FormOps f2 = new FormOps(new Aplicador(Cook.Caminhos[comboOps.SelectedIndex], 
                                                   int.Parse(textIdade.Text)));
            f2.Show();
            f2.Test();
            this.Hide();
        }
    }
}
