namespace Pmfst_GameSDK
{
    partial class BGL
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
            this.syncRate = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnUpute = new System.Windows.Forms.Button();
            this.btnKraj = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // syncRate
            // 
            this.syncRate.AutoSize = true;
            this.syncRate.BackColor = System.Drawing.Color.Transparent;
            this.syncRate.Location = new System.Drawing.Point(12, 394);
            this.syncRate.Name = "syncRate";
            this.syncRate.Size = new System.Drawing.Size(71, 52);
            this.syncRate.TabIndex = 0;
            this.syncRate.Text = "60";
            this.syncRate.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 17;
            this.timer1.Tick += new System.EventHandler(this.Update);
            // 
            // timer2
            // 
            this.timer2.Interval = 250;
            this.timer2.Tick += new System.EventHandler(this.updateFrameRate);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Century Gothic", 25.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTitle.ForeColor = System.Drawing.Color.OliveDrab;
            this.lblTitle.Location = new System.Drawing.Point(234, 82);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(277, 52);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "SUPER ZEKO!";
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnStart.Location = new System.Drawing.Point(259, 146);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(170, 38);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnUpute
            // 
            this.btnUpute.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnUpute.Location = new System.Drawing.Point(259, 187);
            this.btnUpute.Name = "btnUpute";
            this.btnUpute.Size = new System.Drawing.Size(170, 47);
            this.btnUpute.TabIndex = 3;
            this.btnUpute.Text = "Upute";
            this.btnUpute.UseVisualStyleBackColor = true;
            this.btnUpute.Click += new System.EventHandler(this.btnUpute_Click);
            // 
            // btnKraj
            // 
            this.btnKraj.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnKraj.Location = new System.Drawing.Point(259, 271);
            this.btnKraj.Name = "btnKraj";
            this.btnKraj.Size = new System.Drawing.Size(170, 49);
            this.btnKraj.TabIndex = 4;
            this.btnKraj.Text = "Kraj";
            this.btnKraj.UseVisualStyleBackColor = true;
            this.btnKraj.Click += new System.EventHandler(this.btnKraj_Click);
            // 
            // BGL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(26F, 52F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(700, 498);
            this.Controls.Add(this.btnKraj);
            this.Controls.Add(this.btnUpute);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.syncRate);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.OliveDrab;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.MaximizeBox = false;
            this.Name = "BGL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game SDK 2.0";
            this.Load += new System.EventHandler(this.startTimer);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Draw);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseClicked);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label syncRate;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnUpute;
        private System.Windows.Forms.Button btnKraj;
    }
}

