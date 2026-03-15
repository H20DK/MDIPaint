namespace MDIPaint
{
    partial class PluginManagerForm
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
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            dgvPlugins = new System.Windows.Forms.DataGridView();
            Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvPlugins).BeginInit();
            SuspendLayout();
            // 
            // btnOK
            // 
            btnOK.Location = new System.Drawing.Point(129, 255);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 23);
            btnOK.TabIndex = 1;
            btnOK.Text = "Сохранить";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(251, 255);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Отменить";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // dgvPlugins
            // 
            dgvPlugins.AllowUserToAddRows = false;
            dgvPlugins.AllowUserToDeleteRows = false;
            dgvPlugins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPlugins.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Column1, Column2, Column3, Column4 });
            dgvPlugins.GridColor = System.Drawing.Color.WhiteSmoke;
            dgvPlugins.Location = new System.Drawing.Point(-6, -1);
            dgvPlugins.Name = "dgvPlugins";
            dgvPlugins.Size = new System.Drawing.Size(511, 317);
            dgvPlugins.TabIndex = 3;
            // 
            // Column1
            // 
            Column1.HeaderText = "Название";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            // 
            // Column2
            // 
            Column2.HeaderText = "Автор";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // Column3
            // 
            Column3.HeaderText = "Версия";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            // 
            // Column4
            // 
            Column4.HeaderText = "Выбрать";
            Column4.Name = "Column4";
            Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // PluginManagerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(504, 317);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(dgvPlugins);
            Name = "PluginManagerForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "PluginManagerForm";
            Load += PluginManagerForm_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPlugins).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvPlugins;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
    }
}