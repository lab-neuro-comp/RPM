using System;
using System.Windows.Forms;
using Raven.Controller;
using Raven.Model.Calculator;

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
                
                if (comboOps.SelectedIndex < 0)
                    throw new TesteNaoSelecionadoException();

                teste = Cook.Caminhos[comboOps.SelectedIndex];
                int minimum = Calculator.GetMinimumAge(Aplicador.ExtrairTabela(teste, "percentile")); // IDEA Mover essas funções para um local apropriado
                int maximum = Calculator.GetMaximumAge(Aplicador.ExtrairTabela(teste, "percentile"));

                if ((idade < minimum) || (idade > maximum))
                    throw new IdadeInvalidaException();

                if (nome.Length < 1)
                    throw new CamposNaoPreenchidosException();
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Digite uma idade válida.", "Aviso!");
                return;
            }
            catch (IdadeInvalidaException iie)
            {
                MessageBox.Show("Idade fora dos padrões do teste.", "Aviso!");
                return;
            }
            catch (CamposNaoPreenchidosException cnpe)
            {
                MessageBox.Show("Digite um nome para o participante.", "Aviso!");
                return;
            }
            catch (TesteNaoSelecionadoException tnse)
            {
                MessageBox.Show("Escolha um teste.", "Aviso!");
                return;
            }

            FormPre form = new FormPre(this, new Aplicador(nome, teste, idade));
            form.Show();
            this.Hide();
        }

        private void comboOps_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
