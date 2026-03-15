namespace MDIPaint
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            новыйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            сохранитьКакToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            рисунокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            размерХолстаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            окноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            каскадомToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            слеваНаправоToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            сверхуВнизToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            упорядочитьЗначкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            оПрограммеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            фильтрыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            красныйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            синийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            зеленыйToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            другойToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            brushSizeTextBox = new System.Windows.Forms.ToolStripTextBox();
            pencilBtn = new System.Windows.Forms.ToolStripButton();
            eraserBtn = new System.Windows.Forms.ToolStripButton();
            textBtn = new System.Windows.Forms.ToolStripButton();
            bucketBtn = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            lineBtn = new System.Windows.Forms.ToolStripButton();
            ellipseBtn = new System.Windows.Forms.ToolStripButton();
            arrowBtn = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            zoomInBtn = new System.Windows.Forms.ToolStripButton();
            zoomOutBtn = new System.Windows.Forms.ToolStripButton();
            resetViewBtn = new System.Windows.Forms.ToolStripButton();
            checkBox1 = new System.Windows.Forms.CheckBox();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            sslCursorPos = new System.Windows.Forms.ToolStripStatusLabel();
            sslImageSize = new System.Windows.Forms.ToolStripStatusLabel();
            sslTool = new System.Windows.Forms.ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { файлToolStripMenuItem, рисунокToolStripMenuItem, окноToolStripMenuItem, справкаToolStripMenuItem, фильтрыToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.MdiWindowListItem = окноToolStripMenuItem;
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(933, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { новыйToolStripMenuItem, открытьToolStripMenuItem, toolStripSeparator1, сохранитьToolStripMenuItem, сохранитьКакToolStripMenuItem, toolStripSeparator2, выходToolStripMenuItem });
            файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            файлToolStripMenuItem.Text = "&Файл";
            файлToolStripMenuItem.Click += файлToolStripMenuItem_Click;
            // 
            // новыйToolStripMenuItem
            // 
            новыйToolStripMenuItem.Name = "новыйToolStripMenuItem";
            новыйToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
            новыйToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            новыйToolStripMenuItem.Text = "Новый";
            новыйToolStripMenuItem.Click += новыйToolStripMenuItem_Click;
            // 
            // открытьToolStripMenuItem
            // 
            открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            открытьToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            открытьToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            открытьToolStripMenuItem.Text = "Открыть...";
            открытьToolStripMenuItem.Click += открытьToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(231, 6);
            // 
            // сохранитьToolStripMenuItem
            // 
            сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            сохранитьToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            сохранитьToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            сохранитьToolStripMenuItem.Text = "Сохранить";
            сохранитьToolStripMenuItem.Click += сохранитьToolStripMenuItem_Click;
            // 
            // сохранитьКакToolStripMenuItem
            // 
            сохранитьКакToolStripMenuItem.Name = "сохранитьКакToolStripMenuItem";
            сохранитьКакToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S;
            сохранитьКакToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            сохранитьКакToolStripMenuItem.Text = "Сохранить как...";
            сохранитьКакToolStripMenuItem.Click += сохранитьКакToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(231, 6);
            // 
            // выходToolStripMenuItem
            // 
            выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            выходToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            выходToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            выходToolStripMenuItem.Text = "Выход";
            выходToolStripMenuItem.Click += выходToolStripMenuItem_Click;
            // 
            // рисунокToolStripMenuItem
            // 
            рисунокToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { размерХолстаToolStripMenuItem });
            рисунокToolStripMenuItem.Name = "рисунокToolStripMenuItem";
            рисунокToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            рисунокToolStripMenuItem.Text = "&Рисунок";
            рисунокToolStripMenuItem.DropDownOpening += рисунокToolStripMenuItem_DropDownOpening;
            // 
            // размерХолстаToolStripMenuItem
            // 
            размерХолстаToolStripMenuItem.Name = "размерХолстаToolStripMenuItem";
            размерХолстаToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R;
            размерХолстаToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            размерХолстаToolStripMenuItem.Text = "Размер холста...";
            размерХолстаToolStripMenuItem.Click += размерХолстаToolStripMenuItem_Click;
            // 
            // окноToolStripMenuItem
            // 
            окноToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { каскадомToolStripMenuItem, слеваНаправоToolStripMenuItem, сверхуВнизToolStripMenuItem, упорядочитьЗначкиToolStripMenuItem });
            окноToolStripMenuItem.Name = "окноToolStripMenuItem";
            окноToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            окноToolStripMenuItem.Text = "&Окно";
            окноToolStripMenuItem.Click += окноToolStripMenuItem_Click;
            // 
            // каскадомToolStripMenuItem
            // 
            каскадомToolStripMenuItem.Name = "каскадомToolStripMenuItem";
            каскадомToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            каскадомToolStripMenuItem.Text = "Каскадом";
            каскадомToolStripMenuItem.Click += каскадомToolStripMenuItem_Click;
            // 
            // слеваНаправоToolStripMenuItem
            // 
            слеваНаправоToolStripMenuItem.Name = "слеваНаправоToolStripMenuItem";
            слеваНаправоToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            слеваНаправоToolStripMenuItem.Text = "Слева направо";
            слеваНаправоToolStripMenuItem.Click += слеваНаправоToolStripMenuItem_Click;
            // 
            // сверхуВнизToolStripMenuItem
            // 
            сверхуВнизToolStripMenuItem.Name = "сверхуВнизToolStripMenuItem";
            сверхуВнизToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            сверхуВнизToolStripMenuItem.Text = "Сверху вниз";
            сверхуВнизToolStripMenuItem.Click += сверхуВнизToolStripMenuItem_Click;
            // 
            // упорядочитьЗначкиToolStripMenuItem
            // 
            упорядочитьЗначкиToolStripMenuItem.Name = "упорядочитьЗначкиToolStripMenuItem";
            упорядочитьЗначкиToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            упорядочитьЗначкиToolStripMenuItem.Text = "Упорядочить значки";
            упорядочитьЗначкиToolStripMenuItem.Click += упорядочитьЗначкиToolStripMenuItem_Click;
            // 
            // справкаToolStripMenuItem
            // 
            справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { оПрограммеToolStripMenuItem });
            справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            справкаToolStripMenuItem.Text = "&Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            оПрограммеToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            оПрограммеToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            оПрограммеToolStripMenuItem.Text = "О программе...";
            оПрограммеToolStripMenuItem.Click += оПрограммеToolStripMenuItem_Click;
            // 
            // фильтрыToolStripMenuItem
            // 
            фильтрыToolStripMenuItem.Name = "фильтрыToolStripMenuItem";
            фильтрыToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            фильтрыToolStripMenuItem.Text = "Фильтры";
            фильтрыToolStripMenuItem.Click += фильтрыToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripDropDownButton1, toolStripSeparator3, toolStripLabel1, brushSizeTextBox, pencilBtn, eraserBtn, textBtn, bucketBtn, toolStripSeparator4, lineBtn, ellipseBtn, arrowBtn, toolStripSeparator5, zoomInBtn, zoomOutBtn, resetViewBtn });
            toolStrip1.Location = new System.Drawing.Point(0, 24);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 3;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { красныйToolStripMenuItem, синийToolStripMenuItem, зеленыйToolStripMenuItem, другойToolStripMenuItem });
            toolStripDropDownButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            toolStripDropDownButton1.Text = "Цвета";
            // 
            // красныйToolStripMenuItem
            // 
            красныйToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("красныйToolStripMenuItem.Image");
            красныйToolStripMenuItem.Name = "красныйToolStripMenuItem";
            красныйToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            красныйToolStripMenuItem.Text = "Красный";
            красныйToolStripMenuItem.Click += красныйToolStripMenuItem_Click;
            // 
            // синийToolStripMenuItem
            // 
            синийToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("синийToolStripMenuItem.Image");
            синийToolStripMenuItem.Name = "синийToolStripMenuItem";
            синийToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            синийToolStripMenuItem.Text = "Синий";
            синийToolStripMenuItem.Click += синийToolStripMenuItem_Click;
            // 
            // зеленыйToolStripMenuItem
            // 
            зеленыйToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("зеленыйToolStripMenuItem.Image");
            зеленыйToolStripMenuItem.Name = "зеленыйToolStripMenuItem";
            зеленыйToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            зеленыйToolStripMenuItem.Text = "Зеленый";
            зеленыйToolStripMenuItem.Click += зеленыйToolStripMenuItem_Click;
            // 
            // другойToolStripMenuItem
            // 
            другойToolStripMenuItem.Name = "другойToolStripMenuItem";
            другойToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            другойToolStripMenuItem.Text = "Другой...";
            другойToolStripMenuItem.Click += другойToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(41, 22);
            toolStripLabel1.Text = "Кисть:";
            // 
            // brushSizeTextBox
            // 
            brushSizeTextBox.Name = "brushSizeTextBox";
            brushSizeTextBox.Size = new System.Drawing.Size(116, 25);
            brushSizeTextBox.KeyPress += brushSizeTextBox_KeyPress;
            brushSizeTextBox.TextChanged += brushSizeTextBox_TextChanged;
            // 
            // pencilBtn
            // 
            pencilBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            pencilBtn.Image = (System.Drawing.Image)resources.GetObject("pencilBtn.Image");
            pencilBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            pencilBtn.Name = "pencilBtn";
            pencilBtn.Size = new System.Drawing.Size(23, 22);
            pencilBtn.Text = "Карандаш";
            pencilBtn.Click += pencilBtn_Click;
            // 
            // eraserBtn
            // 
            eraserBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            eraserBtn.Image = (System.Drawing.Image)resources.GetObject("eraserBtn.Image");
            eraserBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            eraserBtn.Name = "eraserBtn";
            eraserBtn.Size = new System.Drawing.Size(23, 22);
            eraserBtn.Text = "Ластик";
            eraserBtn.Click += eraserBtn_Click;
            // 
            // textBtn
            // 
            textBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            textBtn.Image = (System.Drawing.Image)resources.GetObject("textBtn.Image");
            textBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            textBtn.Name = "textBtn";
            textBtn.Size = new System.Drawing.Size(23, 22);
            textBtn.Text = "Текст";
            textBtn.Click += textBtn_Click;
            // 
            // bucketBtn
            // 
            bucketBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            bucketBtn.Image = (System.Drawing.Image)resources.GetObject("bucketBtn.Image");
            bucketBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            bucketBtn.Name = "bucketBtn";
            bucketBtn.Size = new System.Drawing.Size(23, 22);
            bucketBtn.Text = "Заливка";
            bucketBtn.Click += bucketBtn_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // lineBtn
            // 
            lineBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            lineBtn.Image = (System.Drawing.Image)resources.GetObject("lineBtn.Image");
            lineBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            lineBtn.Name = "lineBtn";
            lineBtn.Size = new System.Drawing.Size(23, 22);
            lineBtn.Text = "Линия";
            lineBtn.Click += lineBtn_Click;
            // 
            // ellipseBtn
            // 
            ellipseBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            ellipseBtn.Image = (System.Drawing.Image)resources.GetObject("ellipseBtn.Image");
            ellipseBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            ellipseBtn.Name = "ellipseBtn";
            ellipseBtn.Size = new System.Drawing.Size(23, 22);
            ellipseBtn.Text = "Эллипс";
            ellipseBtn.Click += ellipseBtn_Click;
            // 
            // arrowBtn
            // 
            arrowBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            arrowBtn.Image = (System.Drawing.Image)resources.GetObject("arrowBtn.Image");
            arrowBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            arrowBtn.Name = "arrowBtn";
            arrowBtn.Size = new System.Drawing.Size(23, 22);
            arrowBtn.Text = "Стрелка";
            arrowBtn.Click += arrowBtn_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // zoomInBtn
            // 
            zoomInBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            zoomInBtn.Image = (System.Drawing.Image)resources.GetObject("zoomInBtn.Image");
            zoomInBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            zoomInBtn.Name = "zoomInBtn";
            zoomInBtn.Size = new System.Drawing.Size(23, 22);
            zoomInBtn.Text = "Увеличить";
            zoomInBtn.Click += zoomInBtn_Click;
            // 
            // zoomOutBtn
            // 
            zoomOutBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            zoomOutBtn.Image = (System.Drawing.Image)resources.GetObject("zoomOutBtn.Image");
            zoomOutBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            zoomOutBtn.Name = "zoomOutBtn";
            zoomOutBtn.Size = new System.Drawing.Size(23, 22);
            zoomOutBtn.Text = "Уменьшить";
            zoomOutBtn.Click += zoomOutBtn_Click;
            // 
            // resetViewBtn
            // 
            resetViewBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resetViewBtn.Image = (System.Drawing.Image)resources.GetObject("resetViewBtn.Image");
            resetViewBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            resetViewBtn.Name = "resetViewBtn";
            resetViewBtn.Size = new System.Drawing.Size(23, 22);
            resetViewBtn.Text = "Сбросить масштаб";
            resetViewBtn.Click += resetViewBtn_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(763, 31);
            checkBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(144, 19);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Закрашенная фигура";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { sslCursorPos, sslImageSize, sslTool });
            statusStrip1.Location = new System.Drawing.Point(0, 497);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(933, 22);
            statusStrip1.TabIndex = 7;
            statusStrip1.Text = "statusStrip1";
            // 
            // sslCursorPos
            // 
            sslCursorPos.Name = "sslCursorPos";
            sslCursorPos.Size = new System.Drawing.Size(103, 17);
            sslCursorPos.Text = "Позиция курсора";
            // 
            // sslImageSize
            // 
            sslImageSize.Name = "sslImageSize";
            sslImageSize.Size = new System.Drawing.Size(86, 17);
            sslImageSize.Text = "Размер холста";
            sslImageSize.Click += sslImageSize_Click;
            // 
            // sslTool
            // 
            sslTool.Name = "sslTool";
            sslTool.Size = new System.Drawing.Size(74, 17);
            sslTool.Text = "Инструмент";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(statusStrip1);
            Controls.Add(checkBox1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            DoubleBuffered = true;
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Мой Paint";
            MdiChildActivate += MainForm_MdiChildActivate;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem новыйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem рисунокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem окноToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьКакToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem размерХолстаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem каскадомToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem слеваНаправоToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сверхуВнизToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem упорядочитьЗначкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem оПрограммеToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem красныйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem синийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem зеленыйToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem другойToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox brushSizeTextBox;
        private System.Windows.Forms.ToolStripButton pencilBtn;
        private System.Windows.Forms.ToolStripButton lineBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolStripButton ellipseBtn;
        private System.Windows.Forms.ToolStripButton eraserBtn;
        private System.Windows.Forms.ToolStripButton textBtn;
        private System.Windows.Forms.ToolStripButton bucketBtn;
        private System.Windows.Forms.ToolStripButton arrowBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton zoomInBtn;
        private System.Windows.Forms.ToolStripButton zoomOutBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel sslCursorPos;
        private System.Windows.Forms.ToolStripStatusLabel sslImageSize;
        private System.Windows.Forms.ToolStripStatusLabel sslTool;
        private System.Windows.Forms.ToolStripButton resetViewBtn;
        private System.Windows.Forms.ToolStripMenuItem фильтрыToolStripMenuItem;
    }
}

