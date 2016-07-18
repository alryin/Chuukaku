namespace Peasant
{
    partial class StartingForm
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartingForm));
            this.ReadData = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.OpenCsv = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.legenda = new System.Windows.Forms.Label();
            this.legenda_2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.zedGraphControl2 = new ZedGraph.ZedGraphControl();
            this.zedGraphControl3 = new ZedGraph.ZedGraphControl();
            this.zedGraphControl4 = new ZedGraph.ZedGraphControl();
            this.zedGraphControl5 = new ZedGraph.ZedGraphControl();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SampCsv = new System.Windows.Forms.CheckBox();
            this.EventsCsv = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReadData
            // 
            this.ReadData.Location = new System.Drawing.Point(16, 47);
            this.ReadData.Name = "ReadData";
            this.ReadData.Size = new System.Drawing.Size(81, 20);
            this.ReadData.TabIndex = 0;
            this.ReadData.Text = "Read Data";
            this.ReadData.UseVisualStyleBackColor = true;
            this.ReadData.Click += new System.EventHandler(this.ReadData_Click);
            // 
            // Stop
            // 
            this.Stop.Enabled = false;
            this.Stop.Location = new System.Drawing.Point(156, 47);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 19);
            this.Stop.TabIndex = 11;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // OpenCsv
            // 
            this.OpenCsv.Enabled = false;
            this.OpenCsv.Location = new System.Drawing.Point(6, 13);
            this.OpenCsv.Name = "OpenCsv";
            this.OpenCsv.Size = new System.Drawing.Size(80, 21);
            this.OpenCsv.TabIndex = 15;
            this.OpenCsv.Text = "Open csv";
            this.OpenCsv.UseVisualStyleBackColor = true;
            this.OpenCsv.Click += new System.EventHandler(this.OpenCsv_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(16, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(37, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "45555";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(59, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port number";
            // 
            // legenda
            // 
            this.legenda.BackColor = System.Drawing.Color.Ivory;
            this.legenda.ForeColor = System.Drawing.Color.Black;
            this.legenda.Location = new System.Drawing.Point(890, 34);
            this.legenda.Name = "legenda";
            this.legenda.Size = new System.Drawing.Size(55, 16);
            this.legenda.TabIndex = 17;
            this.legenda.Text = "Legenda: ";
            this.legenda.MouseLeave += new System.EventHandler(this.legenda_MouseLeave);
            this.legenda.MouseHover += new System.EventHandler(this.legenda_MouseHover);
            // 
            // legenda_2
            // 
            this.legenda_2.BackColor = System.Drawing.Color.Ivory;
            this.legenda_2.ForeColor = System.Drawing.Color.Black;
            this.legenda_2.Location = new System.Drawing.Point(371, 34);
            this.legenda_2.Name = "legenda_2";
            this.legenda_2.Size = new System.Drawing.Size(55, 16);
            this.legenda_2.TabIndex = 18;
            this.legenda_2.Text = "Legenda: ";
            this.legenda_2.MouseLeave += new System.EventHandler(this.legenda_2_MouseLeave);
            this.legenda_2.MouseHover += new System.EventHandler(this.legenda_2_MouseHover);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(202, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Sensor Number";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown1.Location = new System.Drawing.Point(156, 12);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(40, 20);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.AutoSize = true;
            this.zedGraphControl1.BackColor = System.Drawing.Color.Transparent;
            this.zedGraphControl1.IsAutoScrollRange = true;
            this.zedGraphControl1.IsShowHScrollBar = true;
            this.zedGraphControl1.Location = new System.Drawing.Point(16, 255);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(300, 230);
            this.zedGraphControl1.TabIndex = 3;
            // 
            // zedGraphControl2
            // 
            this.zedGraphControl2.AutoSize = true;
            this.zedGraphControl2.BackColor = System.Drawing.Color.Transparent;
            this.zedGraphControl2.IsAutoScrollRange = true;
            this.zedGraphControl2.IsShowHScrollBar = true;
            this.zedGraphControl2.Location = new System.Drawing.Point(335, 253);
            this.zedGraphControl2.Name = "zedGraphControl2";
            this.zedGraphControl2.ScrollGrace = 0D;
            this.zedGraphControl2.ScrollMaxX = 0D;
            this.zedGraphControl2.ScrollMaxY = 0D;
            this.zedGraphControl2.ScrollMaxY2 = 0D;
            this.zedGraphControl2.ScrollMinX = 0D;
            this.zedGraphControl2.ScrollMinY = 0D;
            this.zedGraphControl2.ScrollMinY2 = 0D;
            this.zedGraphControl2.Size = new System.Drawing.Size(300, 230);
            this.zedGraphControl2.TabIndex = 6;
            // 
            // zedGraphControl3
            // 
            this.zedGraphControl3.AutoSize = true;
            this.zedGraphControl3.BackColor = System.Drawing.Color.Transparent;
            this.zedGraphControl3.IsAutoScrollRange = true;
            this.zedGraphControl3.IsShowHScrollBar = true;
            this.zedGraphControl3.Location = new System.Drawing.Point(657, 253);
            this.zedGraphControl3.Name = "zedGraphControl3";
            this.zedGraphControl3.ScrollGrace = 0D;
            this.zedGraphControl3.ScrollMaxX = 0D;
            this.zedGraphControl3.ScrollMaxY = 0D;
            this.zedGraphControl3.ScrollMaxY2 = 0D;
            this.zedGraphControl3.ScrollMinX = 0D;
            this.zedGraphControl3.ScrollMinY = 0D;
            this.zedGraphControl3.ScrollMinY2 = 0D;
            this.zedGraphControl3.Size = new System.Drawing.Size(300, 230);
            this.zedGraphControl3.TabIndex = 7;
            // 
            // zedGraphControl4
            // 
            this.zedGraphControl4.AutoSize = true;
            this.zedGraphControl4.BackColor = System.Drawing.Color.Transparent;
            this.zedGraphControl4.IsAutoScrollRange = true;
            this.zedGraphControl4.IsShowHScrollBar = true;
            this.zedGraphControl4.Location = new System.Drawing.Point(657, 12);
            this.zedGraphControl4.Name = "zedGraphControl4";
            this.zedGraphControl4.ScrollGrace = 0D;
            this.zedGraphControl4.ScrollMaxX = 0D;
            this.zedGraphControl4.ScrollMaxY = 0D;
            this.zedGraphControl4.ScrollMaxY2 = 0D;
            this.zedGraphControl4.ScrollMinX = 0D;
            this.zedGraphControl4.ScrollMinY = 0D;
            this.zedGraphControl4.ScrollMinY2 = 0D;
            this.zedGraphControl4.Size = new System.Drawing.Size(300, 230);
            this.zedGraphControl4.TabIndex = 8;
            // 
            // zedGraphControl5
            // 
            this.zedGraphControl5.AutoSize = true;
            this.zedGraphControl5.BackColor = System.Drawing.Color.Transparent;
            this.zedGraphControl5.IsAutoScrollRange = true;
            this.zedGraphControl5.IsShowHScrollBar = true;
            this.zedGraphControl5.Location = new System.Drawing.Point(335, 12);
            this.zedGraphControl5.Name = "zedGraphControl5";
            this.zedGraphControl5.ScrollGrace = 0D;
            this.zedGraphControl5.ScrollMaxX = 0D;
            this.zedGraphControl5.ScrollMaxY = 0D;
            this.zedGraphControl5.ScrollMaxY2 = 0D;
            this.zedGraphControl5.ScrollMinX = 0D;
            this.zedGraphControl5.ScrollMinY = 0D;
            this.zedGraphControl5.ScrollMinY2 = 0D;
            this.zedGraphControl5.Size = new System.Drawing.Size(300, 230);
            this.zedGraphControl5.TabIndex = 9;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(16, 84);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(300, 58);
            this.richTextBox1.TabIndex = 10;
            this.richTextBox1.TabStop = false;
            this.richTextBox1.Text = "";
            // 
            // SampCsv
            // 
            this.SampCsv.AutoSize = true;
            this.SampCsv.Location = new System.Drawing.Point(92, 16);
            this.SampCsv.Name = "SampCsv";
            this.SampCsv.Size = new System.Drawing.Size(73, 17);
            this.SampCsv.TabIndex = 12;
            this.SampCsv.Text = "csv Samp";
            this.SampCsv.UseVisualStyleBackColor = true;
            // 
            // EventsCsv
            // 
            this.EventsCsv.AutoSize = true;
            this.EventsCsv.Location = new System.Drawing.Point(171, 16);
            this.EventsCsv.Name = "EventsCsv";
            this.EventsCsv.Size = new System.Drawing.Size(76, 17);
            this.EventsCsv.TabIndex = 13;
            this.EventsCsv.Text = "csv Eventi";
            this.EventsCsv.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OpenCsv);
            this.groupBox1.Controls.Add(this.EventsCsv);
            this.groupBox1.Controls.Add(this.SampCsv);
            this.groupBox1.Location = new System.Drawing.Point(16, 177);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 47);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // StartingForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(980, 497);
            this.Controls.Add(this.legenda_2);
            this.Controls.Add(this.legenda);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.zedGraphControl5);
            this.Controls.Add(this.zedGraphControl4);
            this.Controls.Add(this.zedGraphControl3);
            this.Controls.Add(this.zedGraphControl2);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ReadData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StartingForm";
            this.Text = "GUI";
            this.Load += new System.EventHandler(this.StartingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ReadData;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button OpenCsv;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label legenda;
        private System.Windows.Forms.Label legenda_2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private ZedGraph.ZedGraphControl zedGraphControl2;
        private ZedGraph.ZedGraphControl zedGraphControl3;
        private ZedGraph.ZedGraphControl zedGraphControl4;
        private ZedGraph.ZedGraphControl zedGraphControl5;       
        private System.Windows.Forms.CheckBox SampCsv;
        private System.Windows.Forms.CheckBox EventsCsv;       
        private System.Windows.Forms.GroupBox groupBox1;
        
    }
}

