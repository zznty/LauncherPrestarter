namespace Prestarter
{
    partial class PrestarterForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainProgressBar = new System.Windows.Forms.ProgressBar();
            this.logoLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainProgressBar
            // 
            this.mainProgressBar.Location = new System.Drawing.Point(137, 97);
            this.mainProgressBar.Margin = new System.Windows.Forms.Padding(2);
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(355, 15);
            this.mainProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.mainProgressBar.TabIndex = 0;
            this.mainProgressBar.UseWaitCursor = true;
            // 
            // logoLabel
            // 
            this.logoLabel.AutoSize = true;
            this.logoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logoLabel.Location = new System.Drawing.Point(137, 14);
            this.logoLabel.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.logoLabel.Name = "logoLabel";
            this.logoLabel.Size = new System.Drawing.Size(236, 37);
            this.logoLabel.TabIndex = 1;
            this.logoLabel.Text = "GravitLauncher";
            this.logoLabel.UseWaitCursor = true;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusLabel.Location = new System.Drawing.Point(137, 78);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(108, 15);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Инициализация...";
            this.statusLabel.UseWaitCursor = true;
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.logoPictureBox.Location = new System.Drawing.Point(14, 14);
            this.logoPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(107, 104);
            this.logoPictureBox.TabIndex = 3;
            this.logoPictureBox.TabStop = false;
            this.logoPictureBox.UseWaitCursor = true;
            // 
            // PrestarterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 142);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.logoLabel);
            this.Controls.Add(this.mainProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrestarterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.PrestarterForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PreStartedForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PreStartedForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PreStartedForm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.ProgressBar mainProgressBar;
        private System.Windows.Forms.Label logoLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.PictureBox logoPictureBox;
    }
}

