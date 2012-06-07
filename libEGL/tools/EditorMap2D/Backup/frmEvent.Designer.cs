namespace EditorMapa2D
{
    partial class frmEvent
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDeleteEvent = new System.Windows.Forms.Button();
            this.btnNewEvent = new System.Windows.Forms.Button();
            this.dgEventos = new System.Windows.Forms.DataGridView();
            this.eventCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dsControleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsControle = new EditorMapa2D.dsControle();
            this.btCancelar = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEventos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsControleBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsControle)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDeleteEvent);
            this.groupBox1.Controls.Add(this.btnNewEvent);
            this.groupBox1.Controls.Add(this.dgEventos);
            this.groupBox1.Location = new System.Drawing.Point(7, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 272);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Eventos";
            // 
            // btnDeleteEvent
            // 
            this.btnDeleteEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteEvent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDeleteEvent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteEvent.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDeleteEvent.FlatAppearance.BorderSize = 0;
            this.btnDeleteEvent.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnDeleteEvent.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteEvent.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteEvent.Image = global::EditorMapa2D.Properties.Resources.script_delete;
            this.btnDeleteEvent.Location = new System.Drawing.Point(254, 248);
            this.btnDeleteEvent.Name = "btnDeleteEvent";
            this.btnDeleteEvent.Size = new System.Drawing.Size(17, 17);
            this.btnDeleteEvent.TabIndex = 32;
            this.btnDeleteEvent.UseVisualStyleBackColor = true;
            this.btnDeleteEvent.Click += new System.EventHandler(this.btnDeleteEvent_Click);
            // 
            // btnNewEvent
            // 
            this.btnNewEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewEvent.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnNewEvent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewEvent.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnNewEvent.FlatAppearance.BorderSize = 0;
            this.btnNewEvent.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ControlDark;
            this.btnNewEvent.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btnNewEvent.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnNewEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewEvent.Image = global::EditorMapa2D.Properties.Resources.script_add;
            this.btnNewEvent.Location = new System.Drawing.Point(230, 248);
            this.btnNewEvent.Name = "btnNewEvent";
            this.btnNewEvent.Size = new System.Drawing.Size(17, 17);
            this.btnNewEvent.TabIndex = 31;
            this.btnNewEvent.UseVisualStyleBackColor = true;
            this.btnNewEvent.Click += new System.EventHandler(this.btnNewEvent_Click);
            // 
            // dgEventos
            // 
            this.dgEventos.AllowUserToAddRows = false;
            this.dgEventos.AllowUserToDeleteRows = false;
            this.dgEventos.AllowUserToResizeColumns = false;
            this.dgEventos.AllowUserToResizeRows = false;
            this.dgEventos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgEventos.AutoGenerateColumns = false;
            this.dgEventos.BackgroundColor = System.Drawing.Color.White;
            this.dgEventos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgEventos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgEventos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEventos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.eventCode,
            this.eventName});
            this.dgEventos.DataMember = "events";
            this.dgEventos.DataSource = this.dsControleBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgEventos.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgEventos.EnableHeadersVisualStyles = false;
            this.dgEventos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(233)))), ((int)(((byte)(252)))));
            this.dgEventos.Location = new System.Drawing.Point(9, 18);
            this.dgEventos.MultiSelect = false;
            this.dgEventos.Name = "dgEventos";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgEventos.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgEventos.RowHeadersVisible = false;
            this.dgEventos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgEventos.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(233)))), ((int)(((byte)(252)))));
            this.dgEventos.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgEventos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgEventos.Size = new System.Drawing.Size(262, 224);
            this.dgEventos.TabIndex = 30;
            this.dgEventos.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgEventos_CellEndEdit);
            this.dgEventos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgEventos_CellClick);
            // 
            // eventCode
            // 
            this.eventCode.DataPropertyName = "code";
            this.eventCode.HeaderText = "Código";
            this.eventCode.Name = "eventCode";
            this.eventCode.ReadOnly = true;
            this.eventCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.eventCode.Width = 50;
            // 
            // eventName
            // 
            this.eventName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.eventName.DataPropertyName = "nm_event";
            this.eventName.HeaderText = "Descrição";
            this.eventName.Name = "eventName";
            this.eventName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dsControleBindingSource
            // 
            this.dsControleBindingSource.DataSource = this.dsControle;
            this.dsControleBindingSource.Position = 0;
            // 
            // dsControle
            // 
            this.dsControle.DataSetName = "dsControle";
            this.dsControle.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(212, 283);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 3;
            this.btCancelar.Text = "Fechar";
            this.btCancelar.UseVisualStyleBackColor = true;
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(104, 283);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(99, 23);
            this.btOk.TabIndex = 4;
            this.btOk.Text = "Aplicar Evento";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // frmEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 318);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEvent";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Eventos";
            this.Shown += new System.EventHandler(this.frmEvent_Shown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgEventos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsControleBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsControle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btCancelar;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.DataGridView dgEventos;
        private dsControle dsControle;
        private System.Windows.Forms.BindingSource dsControleBindingSource;
        private System.Windows.Forms.Button btnDeleteEvent;
        private System.Windows.Forms.Button btnNewEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn eventName;
    }
}