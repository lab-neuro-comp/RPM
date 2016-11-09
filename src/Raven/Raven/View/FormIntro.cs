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
         
            // Adicionando opções de testes   
            Cook = new Preparador();
            Cook.CarregarTestes();
            foreach(string item in Cook.Testes)
                comboOps.Items.Add(item);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nome;
            string teste;
            int idade;

            try
            {
                nome = textNome.Text;
                idade = int.Parse(textIdade.Text);
                teste = Cook.Caminhos[comboOps.SelectedIndex];
                
                if (nome.Length < 1)
                    throw new Exception();
            }
            catch (Exception any)
            {
                MessageBox.Show("Preencha todos os campos", "Aviso!");
                return;
            }

            FormPre form = new FormPre(this, new Aplicador(nome, teste, idade));
            form.Show();
            this.Hide();
        }
    }
}
