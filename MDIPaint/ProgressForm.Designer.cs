namespace MDIPaint
{
    partial class ProgressForm
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
            lblStatus = new System.Windows.Forms.Label();
            progressBar = new System.Windows.Forms.ProgressBar();
            btnCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new System.Drawing.Point(286, 76);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(43, 15);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "Статус";
            // 
            // progressBar
            // 
            progressBar.Location = new System.Drawing.Point(250, 135);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(100, 23);
            progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            progressBar.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(315, 236);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Отмена";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // ProgressForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(btnCancel);
            Controls.Add(progressBar);
            Controls.Add(lblStatus);
            Name = "ProgressForm";
            Text = "ProgressForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnCancel;
    }
}