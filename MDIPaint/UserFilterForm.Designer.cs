namespace MDIPaint
{
    partial class UserFilterForm
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
            textBoxCode = new System.Windows.Forms.TextBox();
            btnApply = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // textBoxCode
            // 
            textBoxCode.Location = new System.Drawing.Point(1, -4);
            textBoxCode.Multiline = true;
            textBoxCode.Name = "textBoxCode";
            textBoxCode.Size = new System.Drawing.Size(802, 458);
            textBoxCode.TabIndex = 0;
            // 
            // btnApply
            // 
            btnApply.Location = new System.Drawing.Point(694, 405);
            btnApply.Name = "btnApply";
            btnApply.Size = new System.Drawing.Size(94, 33);
            btnApply.TabIndex = 1;
            btnApply.Text = "Применить";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // UserFilterForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(btnApply);
            Controls.Add(textBoxCode);
            Name = "UserFilterForm";
            Text = "UserFilterForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Button btnApply;
    }
}