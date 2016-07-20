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
        private Aplicador App { get; set; }
        private PictureBox[] Pics { get; set; }
        private int NoRodada { get; set; }
        private bool Respondeu { get; set; }

        public FormOps()
        {
            InitializeComponent();
            Pics = new PictureBox[8];
            this.KeyUp += new KeyEventHandler(this.FormOps_KeyUp);

            Pics[0] = picOp1;
            Pics[1] = picOp2;
            Pics[2] = picOp3;
            Pics[3] = picOp4;
            Pics[4] = picOp5;
            Pics[5] = picOp6;
            Pics[6] = picOp7;
            Pics[7] = picOp8;
        }

        public FormOps(Aplicador app) : this()
        {
            this.App = app;
        }

        public async void Test()
        {
            Stopwatch clock = new Stopwatch();

            App.PrepararTeste();
            for (NoRodada = 0; NoRodada < this.App.Imagens.Length; NoRodada++)
            {
                DefinirTela(this.App.CarregarImagens(NoRodada));
                clock.Start();
                await RecebeuResposta();
                clock.Stop();

                App.OuvirDuracao(NoRodada, clock.ElapsedMilliseconds);
                clock.Reset();
            }

            // Termina o teste
            FormResultado form = new FormResultado(App.CalcularResultado(), 
                                                   App.NoRespostasCorretas);
            Console.WriteLine("Sai do loop");
            form.Show();
            Close();
        }

        private void DefinirTela(string[] imagens)
        {
            int no_ops = AjustarOpcoes(imagens.Length-1);

            picMain.Image = Image.FromFile(imagens[0]);
            for (int i = 1; i <= no_ops; ++i)
            {
                Pics[i - 1].Image = Image.FromFile(imagens[i]);
            }

            Show();
        }

        private int AjustarOpcoes(int length)
        {
            int width = 0;
            int left = 0;
            bool enabled = true;

            switch (length)
            {
                case 6:
                    width = 463;
                    left = 65;
                    enabled = false;
                    break;

                case 8:
                    width = 600;
                    left = 131;
                    enabled = true;
                    break;
            }

            Width = width;
            picMain.Location = new Point(left, 12);
            picOp7.Enabled = picOp7.Visible = enabled;
            picOp8.Enabled = picOp8.Visible = enabled;
            return length;
        }

        private void ReceberResposta(int resposta)
        {
            this.App.OuvirResposta(NoRodada, resposta);
        }

        private async Task<string> RecebeuResposta()
        {
            Respondeu = false;

            while (!Respondeu)
            {
                await TaskEx.Delay(10);
            }

            return "Respondido";
        }

        #region Description of user input
        private void FormOps_KeyUp(object sender, KeyEventArgs e)
        {
            int resposta = 0;

            switch (e.KeyCode.ToString())
            {
                case "NumPad1":
                    resposta = 1;
                    break;
                case "NumPad2":
                    resposta = 2;
                    break;
                case "NumPad3":
                    resposta = 3;
                    break;
                case "NumPad4":
                    resposta = 4;
                    break;
                case "NumPad5":
                    resposta = 5;
                    break;
                case "NumPad6":
                    resposta = 6;
                    break;
                case "NumPad7":
                    resposta = 7;
                    break;
                case "Numpad8":
                    resposta = 8;
                    break;
            }

            if (resposta > 0)
            {
                Respondeu = true;
                App.OuvirResposta(NoRodada, resposta);
            }

        }

        #endregion

        #region Pictures callbacks
        private void picOp1_Click(object sender, EventArgs e)
        {
            Respondeu = true;
            App.OuvirResposta(NoRodada, 1);
        }

        private void picOp2_Click(object sender, EventArgs e)
        {
            Respondeu = true;
            App.OuvirResposta(NoRodada, 2);
        }

        private void picOp3_Click(object sender, EventArgs e)
        {
            Respondeu = true;
            App.OuvirResposta(NoRodada, 3);
        }

        private void picOp4_Click(object sender, EventArgs e)
        {
            Respondeu = true;
            App.OuvirResposta(NoRodada, 4);
        }

        private void picOp5_Click(object sender, EventArgs e)
        {
            Respondeu = true;
            App.OuvirResposta(NoRodada, 5);
        }

        private void picOp6_Click(object sender, EventArgs e)
        {
            Respondeu = true;
            App.OuvirResposta(NoRodada, 6);
        }

        private void picOp7_Click(object sender, EventArgs e)
        {
            Respondeu = true;
            App.OuvirResposta(NoRodada, 7);
        }

        private void picOp8_Click(object sender, EventArgs e)
        {
            Respondeu = true;
            App.OuvirResposta(NoRodada, 8);
        }
        #endregion
    }
}
