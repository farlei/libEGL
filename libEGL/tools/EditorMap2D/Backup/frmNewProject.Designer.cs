namespace EditorMapa2D
{
    partial class frmNewProject
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
            this.nudTileWidth = new System.Windows.Forms.NumericUpDown();
            this.nudTileHeight = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudMapWidth = new System.Windows.Forms.NumericUpDown();
            this.nudMapHeight = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudTileWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTileHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMapWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMapHeight)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudTileWidth
            // 
            this.nudTileWidth.Location = new System.Drawing.Point(65, 53);
            this.nudTileWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudTileWidth.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudTileWidth.Name = "nudTileWidth";
            this.nudTileWidth.Size = new System.Drawing.Size(50, 20);
            this.nudTileWidth.TabIndex = 24;
            this.nudTileWidth.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nudTileWidth.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // nudTileHeight
            // 
            this.nudTileHeight.Location = new System.Drawing.Point(192, 53);
            this.nudTileHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudTileHeight.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudTileHeight.Name = "nudTileHeight";
            this.nudTileHeight.Size = new System.Drawing.Size(50, 20);
            this.nudTileHeight.TabIndex = 26;
            this.nudTileHeight.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nudTileHeight.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Largura:                    px";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(152, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Altura:                    px";
            // 
            // nudMapWidth
            // 
            this.nudMapWidth.Location = new System.Drawing.Point(65, 55);
            this.nudMapWidth.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudMapWidth.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudMapWidth.Name = "nudMapWidth";
            this.nudMapWidth.Size = new System.Drawing.Size(50, 20);
            this.nudMapWidth.TabIndex = 28;
            this.nudMapWidth.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudMapWidth.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // nudMapHeight
            // 
            this.nudMapHeight.Location = new System.Drawing.Point(177, 55);
            this.nudMapHeight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudMapHeight.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudMapHeight.Name = "nudMapHeight";
            this.nudMapHeight.Size = new System.Drawing.Size(50, 20);
            this.nudMapHeight.TabIndex = 30;
            this.nudMapHeight.Value = new decimal(new int[] {
            19,
            0,
            0,
            0});
            this.nudMapHeight.ValueChanged += new System.EventHandler(this.nud_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Largura:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(137, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Altura:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Selecione o tamanho que será o tile no mapa.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nudTileWidth);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.nudTileHeight);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 100);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tile info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.nudMapWidth);
            this.groupBox2.Controls.Add(this.nudMapHeight);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(273, 100);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mapa info (800x608)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(197, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Selecione o número de tile para o mapa.";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(210, 231);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 29;
            this.btnClose.Text = "Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(128, 231);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 30;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmNewProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(297, 264);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNewProject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Novo projeto";
            ((System.ComponentModel.ISupportInitialize)(this.nudTileWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTileHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMapWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMapHeight)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudTileWidth;
        private System.Windows.Forms.NumericUpDown nudTileHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudMapWidth;
        private System.Windows.Forms.NumericUpDown nudMapHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
    }
}