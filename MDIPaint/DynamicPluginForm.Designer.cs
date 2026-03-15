namespace MDIPaint
{
    partial class DynamicPluginForm
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
            txtCode = new System.Windows.Forms.TextBox();
            btnCompile = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            txtPluginName = new System.Windows.Forms.TextBox();
            btnCancel = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // txtCode
            // 
            txtCode.Location = new System.Drawing.Point(-3, 56);
            txtCode.Multiline = true;
            txtCode.Name = "txtCode";
            txtCode.Size = new System.Drawing.Size(791, 382);
            txtCode.TabIndex = 0;
            // 
            // btnCompile
            // 
            btnCompile.Location = new System.Drawing.Point(405, 17);
            btnCompile.Name = "btnCompile";
            btnCompile.Size = new System.Drawing.Size(104, 23);
            btnCompile.TabIndex = 1;
            btnCompile.Text = "Применить";
            btnCompile.UseVisualStyleBackColor = true;
            btnCompile.Click += btnCompile_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 17);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(82, 15);
            label1.TabIndex = 2;
            label1.Text = "Имя плагина:";
            // 
            // txtPluginName
            // 
            txtPluginName.Location = new System.Drawing.Point(100, 14);
            txtPluginName.Name = "txtPluginName";
            txtPluginName.Size = new System.Drawing.Size(100, 23);
            txtPluginName.TabIndex = 3;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(581, 17);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Отменить";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // DynamicPluginForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(btnCancel);
            Controls.Add(txtPluginName);
            Controls.Add(label1);
            Controls.Add(btnCompile);
            Controls.Add(txtCode);
            Name = "DynamicPluginForm";
            Text = "DynamicPluginForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnCompile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPluginName;
        private System.Windows.Forms.Button btnCancel;
    }
}