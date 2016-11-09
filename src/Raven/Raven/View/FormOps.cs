using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Raven.Controller;

namespace Raven.View
{
    public partial class FormOps : Form
    {
        private FormIntro Mother { get; set; }
        private Aplicador App { get; set; }
        private PictureBox[] Pics { get; set; }
        private Label[] Labels { get; set; }
        private int NoRodada { get; set; }
        private bool Respondeu { get; set; }
        private int NoRespostas { get; set; }

        public FormOps(FormIntro mother)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            this.KeyUp += new KeyEventHandler(this.FormOps_KeyUp);
            Pics = new PictureBox[8];
            Labels = new Label[8];
            Pics[0] = picOp1;
            Pics[1] = picOp2;
            Pics[2] = picOp3;
            Pics[3] = picOp4;
            Pics[4] = picOp5;
            Pics[5] = picOp6;
            Pics[6] = picOp7;
            Pics[7] = picOp8;
            Labels[0] = label1;
            Labels[1] = label2;
            Labels[2] = label3;
            Labels[3] = label4;
            Labels[4] = label5;
            Labels[5] = label6;
            Labels[6] = label7;
            Labels[7] = label8;
            NoRespostas = 6;
            Mother = mother;
        }

        public FormOps(FormIntro mother, Aplicador app) : this(mother)
        {
            this.App = app;
        }

        public async void Test()
        {
            Stopwatch clock = new Stopwatch();

            // Realizando o teste
            App.PrepararTeste();
            for (NoRodada = 0; NoRodada < this.App.Imagens.Length; NoRodada++)
            {
                DefinirTela(App.CarregarImagens(NoRodada));
                clock.Start();
                await RecebeuResposta();
                clock.Stop();

                App.OuvirDuracao(NoRodada, clock.ElapsedMilliseconds);
                clock.Reset();
            }

            // Terminando o teste
            FormOutro form = new FormOutro(Mother, App);
            Mother = null;
            form.Show();
            Close();
        }

        private void DefinirTela(string[] imagens)
        {
            NoRespostas = AjustarOpcoes(imagens.Length-1);
            picMain.Image = Image.FromFile(imagens[0]);

            for (int i = 1; i <= NoRespostas; ++i)
            {
                Pics[i - 1].Image = Image.FromFile(imagens[i]);
            }

            Show();
        }

        private int AjustarOpcoes(int tamanho)
        {
            bool enabled = true;

            switch (tamanho)
            {
                case 6:
                    DefinirNoColunas(tamanho);
                    enabled = false;
                    break;

                case 8:
                    DefinirNoColunas(tamanho);
                    enabled = true;
                    break;
            }

            picOp7.Enabled = picOp7.Visible = enabled;
            picOp8.Enabled = picOp8.Visible = enabled;
            return tamanho;
        }

        private void ReceberResposta(int resposta)
        {
            Respondeu = true;
            this.App.OuvirResposta(NoRodada, resposta);
        }

        private async Task<string> RecebeuResposta()
        {
            Respondeu = false;

            while (!Respondeu)
            {
                await Task.Delay(10);
            }

            return "Respondido";
        }

        #region Description of user input
        private void FormOps_KeyUp(object sender, KeyEventArgs e)
        {
            int resposta = 0;
            string tecla = e.KeyCode.ToString();

            for (int i = 1; i <= NoRespostas; ++i)
            {
                var chute = $"NumPad{i}";
                if (chute.Equals(tecla))
                {
                    resposta = i;
                }
            }


            if (resposta > 0)
            {
                ReceberResposta(resposta);
            }

        }

        #endregion

        #region Pictures callbacks
        private void picOp1_Click(object sender, EventArgs e)
        {
            ReceberResposta(1);
        }

        private void picOp2_Click(object sender, EventArgs e)
        {
            ReceberResposta(2);
        }

        private void picOp3_Click(object sender, EventArgs e)
        {
            ReceberResposta(3);
        }

        private void picOp4_Click(object sender, EventArgs e)
        {
            ReceberResposta(4);
        }

        private void picOp5_Click(object sender, EventArgs e)
        {
            ReceberResposta(5);
        }

        private void picOp6_Click(object sender, EventArgs e)
        {
            ReceberResposta(6);
        }

        private void picOp7_Click(object sender, EventArgs e)
        {
            ReceberResposta(7);
        }

        private void picOp8_Click(object sender, EventArgs e)
        {
            ReceberResposta(8);
        }
        #endregion
    }
}
