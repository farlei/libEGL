namespace EGLCort2
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imagem = new System.Windows.Forms.PictureBox();
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.abrir = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.duplicar = new System.Windows.Forms.ToolStripButton();
            this.mover = new System.Windows.Forms.ToolStripButton();
            this.copiar = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.limpa = new System.Windows.Forms.ToolStripButton();
            this.ajuda = new System.Windows.Forms.ToolStripButton();
            this.painel = new System.Windows.Forms.Panel();
            this.lista = new System.Windows.Forms.ListBox();
            this.txtNome = new System.Windows.Forms.ToolStripTextBox();
            this.label_txtNome = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).BeginInit();
            this.toolbar.SuspendLayout();
            this.painel.SuspendLayout();
            this.SuspendLayout();
            // 
            // imagem
            // 
            this.imagem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.imagem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imagem.Location = new System.Drawing.Point(3, 3);
            this.imagem.Name = "imagem";
            this.imagem.Size = new System.Drawing.Size(627, 268);
            this.imagem.TabIndex = 0;
            this.imagem.TabStop = false;
            this.imagem.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imagem_MouseMove);
            this.imagem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imagem_MouseDown);
            this.imagem.Paint += new System.Windows.Forms.PaintEventHandler(this.imagem_Paint);
            this.imagem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imagem_MouseUp);
            // 
            // toolbar
            // 
            this.toolbar.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrir,
            this.toolStripSeparator1,
            this.duplicar,
            this.mover,
            this.copiar,
            this.toolStripSeparator2,
            this.limpa,
            this.ajuda,
            this.toolStripSeparator3,
            this.label_txtNome,
            this.txtNome});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(657, 71);
            this.toolbar.TabIndex = 1;
            this.toolbar.Text = "toolStrip1";
            // 
            // abrir
            // 
            this.abrir.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.abrir.Image = ((System.Drawing.Image)(resources.GetObject("abrir.Image")));
            this.abrir.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.abrir.Name = "abrir";
            this.abrir.Size = new System.Drawing.Size(68, 68);
            this.abrir.Text = "toolStripButton1";
            this.abrir.ToolTipText = "Abrir Imagem";
            this.abrir.Click += new System.EventHandler(this.abrir_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 71);
            // 
            // duplicar
            // 
            this.duplicar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.duplicar.Image = ((System.Drawing.Image)(resources.GetObject("duplicar.Image")));
            this.duplicar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.duplicar.Name = "duplicar";
            this.duplicar.Size = new System.Drawing.Size(68, 68);
            this.duplicar.Text = "toolStripButton1";
            this.duplicar.ToolTipText = "Duplicar Box Selecionado";
            this.duplicar.Click += new System.EventHandler(this.duplicar_Click);
            // 
            // mover
            // 
            this.mover.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mover.Image = ((System.Drawing.Image)(resources.GetObject("mover.Image")));
            this.mover.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mover.Name = "mover";
            this.mover.Size = new System.Drawing.Size(68, 68);
            this.mover.Text = "Mover Box Selecionado";
            this.mover.Click += new System.EventHandler(this.mover_Click);
            // 
            // copiar
            // 
            this.copiar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copiar.Image = ((System.Drawing.Image)(resources.GetObject("copiar.Image")));
            this.copiar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copiar.Name = "copiar";
            this.copiar.Size = new System.Drawing.Size(68, 68);
            this.copiar.Text = "toolStripButton1";
            this.copiar.ToolTipText = "Copiar para o Clipboard";
            this.copiar.Click += new System.EventHandler(this.copiar_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 71);
            // 
            // limpa
            // 
            this.limpa.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.limpa.Image = ((System.Drawing.Image)(resources.GetObject("limpa.Image")));
            this.limpa.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.limpa.Name = "limpa";
            this.limpa.Size = new System.Drawing.Size(68, 68);
            this.limpa.Text = "toolStripButton1";
            this.limpa.ToolTipText = "Limpar";
            this.limpa.Click += new System.EventHandler(this.limpa_Click);
            // 
            // ajuda
            // 
            this.ajuda.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ajuda.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ajuda.Image = ((System.Drawing.Image)(resources.GetObject("ajuda.Image")));
            this.ajuda.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ajuda.Name = "ajuda";
            this.ajuda.Size = new System.Drawing.Size(68, 68);
            this.ajuda.Text = "Ajuda";
            this.ajuda.Click += new System.EventHandler(this.ajuda_Click);
            // 
            // painel
            // 
            this.painel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.painel.AutoScroll = true;
            this.painel.Controls.Add(this.imagem);
            this.painel.Location = new System.Drawing.Point(12, 74);
            this.painel.Name = "painel";
            this.painel.Size = new System.Drawing.Size(633, 274);
            this.painel.TabIndex = 2;
            // 
            // lista
            // 
            this.lista.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lista.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lista.FormattingEnabled = true;
            this.lista.Location = new System.Drawing.Point(15, 354);
            this.lista.Name = "lista";
            this.lista.Size = new System.Drawing.Size(630, 93);
            this.lista.TabIndex = 3;
            this.lista.SelectedIndexChanged += new System.EventHandler(this.lista_SelectedIndexChanged);
            // 
            // txtNome
            // 
            this.txtNome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(100, 71);
            this.txtNome.ToolTipText = "Nome do objeto da classe imagem";
            // 
            // label_txtNome
            // 
            this.label_txtNome.Name = "label_txtNome";
            this.label_txtNome.Size = new System.Drawing.Size(43, 68);
            this.label_txtNome.Text = "Nome:";
            this.label_txtNome.ToolTipText = "Nome do objeto da classe imagem";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 71);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 457);
            this.Controls.Add(this.lista);
            this.Controls.Add(this.painel);
            this.Controls.Add(this.toolbar);
            this.Name = "Form1";
            this.Text = "EGLCort 2";
            ((System.ComponentModel.ISupportInitialize)(this.imagem)).EndInit();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.painel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imagem;
        private System.Windows.Forms.ToolStrip toolbar;
        private System.Windows.Forms.ToolStripButton abrir;
        private System.Windows.Forms.Panel painel;
        private System.Windows.Forms.ListBox lista;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton limpa;
        private System.Windows.Forms.ToolStripButton copiar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton duplicar;
        private System.Windows.Forms.ToolStripButton mover;
        private System.Windows.Forms.ToolStripButton ajuda;
        private System.Windows.Forms.ToolStripTextBox txtNome;
        private System.Windows.Forms.ToolStripLabel label_txtNome;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

