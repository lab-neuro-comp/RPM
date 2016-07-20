namespace Raven.View
{
    partial class FormResultado
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblChaveResultado = new System.Windows.Forms.Label();
            this.lblResultado = new System.Windows.Forms.Label();
            this.btnSair = new System.Windows.Forms.Button();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.labelTituloRespostasCorretas = new System.Windows.Forms.Label();
            this.lblNoRespostasCorretas = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.labelTituloResultado = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblTitulo, 2);
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(3, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(588, 117);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Resultados";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblChaveResultado
            // 
            this.lblChaveResultado.AutoSize = true;
            this.lblChaveResultado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChaveResultado.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChaveResultado.Location = new System.Drawing.Point(3, 117);
            this.lblChaveResultado.Name = "lblChaveResultado";
            this.lblChaveResultado.Size = new System.Drawing.Size(291, 63);
            this.lblChaveResultado.TabIndex = 1;
            this.lblChaveResultado.Text = "Percentil:";
            this.lblChaveResultado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResultado.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultado.Location = new System.Drawing.Point(300, 243);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(291, 63);
            this.lblResultado.TabIndex = 2;
            this.lblResultado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblResultado.Visible = false;
            // 
            // btnSair
            // 
            this.btnSair.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSair.AutoSize = true;
            this.btnSair.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSair.Location = new System.Drawing.Point(76, 352);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(145, 28);
            this.btnSair.TabIndex = 3;
            this.btnSair.Text = "Sair";
            this.btnSair.UseVisualStyleBackColor = true;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnVoltar
            // 
            this.btnVoltar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnVoltar.AutoSize = true;
            this.btnVoltar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoltar.Location = new System.Drawing.Point(366, 352);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(158, 28);
            this.btnVoltar.TabIndex = 4;
            this.btnVoltar.Text = "Recomeçar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // labelTituloRespostasCorretas
            // 
            this.labelTituloRespostasCorretas.AutoSize = true;
            this.labelTituloRespostasCorretas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTituloRespostasCorretas.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTituloRespostasCorretas.Location = new System.Drawing.Point(3, 180);
            this.labelTituloRespostasCorretas.Name = "labelTituloRespostasCorretas";
            this.labelTituloRespostasCorretas.Size = new System.Drawing.Size(291, 63);
            this.labelTituloRespostasCorretas.TabIndex = 6;
            this.labelTituloRespostasCorretas.Text = "Respostas Corretas:";
            this.labelTituloRespostasCorretas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNoRespostasCorretas
            // 
            this.lblNoRespostasCorretas.AutoSize = true;
            this.lblNoRespostasCorretas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNoRespostasCorretas.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoRespostasCorretas.Location = new System.Drawing.Point(300, 180);
            this.lblNoRespostasCorretas.Name = "lblNoRespostasCorretas";
            this.lblNoRespostasCorretas.Size = new System.Drawing.Size(291, 63);
            this.lblNoRespostasCorretas.TabIndex = 4;
            this.lblNoRespostasCorretas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLevel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.Location = new System.Drawing.Point(300, 117);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(291, 63);
            this.lblLevel.TabIndex = 0;
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTituloResultado
            // 
            this.labelTituloResultado.AutoSize = true;
            this.labelTituloResultado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTituloResultado.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTituloResultado.Location = new System.Drawing.Point(3, 243);
            this.labelTituloResultado.Name = "labelTituloResultado";
            this.labelTituloResultado.Size = new System.Drawing.Size(291, 63);
            this.labelTituloResultado.TabIndex = 0;
            this.labelTituloResultado.Text = "Resultado:";
            this.labelTituloResultado.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelTituloResultado.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblLevel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnSair, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblNoRespostasCorretas, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelTituloResultado, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblResultado, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblChaveResultado, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTitulo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTituloRespostasCorretas, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnVoltar, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(594, 426);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // FormResultado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 426);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FormResultado";
            this.Text = "Resultado";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblChaveResultado;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Button btnSair;
        private System.Windows.Forms.Button btnVoltar;
        private System.Windows.Forms.Label labelTituloRespostasCorretas;
        private System.Windows.Forms.Label lblNoRespostasCorretas;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label labelTituloResultado;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}