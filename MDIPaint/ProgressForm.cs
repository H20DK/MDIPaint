using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class ProgressForm : Form
    {
        private readonly CancellationTokenSource _cts;
        private readonly string _operationName;
        public ProgressForm(string operationName = "Применение фильтра")
        {
            InitializeComponent();
            _operationName = operationName;
            _cts = new CancellationTokenSource();

            this.Text = _operationName;
            lblStatus.Text = "Подготовка...";
            progressBar.Style = ProgressBarStyle.Marquee;   // сначала анимированный
            btnCancel.Click += btnCancel_Click;
        }
        public CancellationToken CancellationToken => _cts.Token;

        public void ReportProgress(int percent)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ReportProgress(percent)));
                return;
            }

            // Переключаем стиль, если прогресс начался
            if (percent > 0 && progressBar.Style == ProgressBarStyle.Marquee)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
            }

            progressBar.Value = Math.Clamp(percent, 0, 100);

            //  Если 100%, можно автоматически закрыть форму через небольшую задержку
            if (percent >= 100)
            {
                lblStatus.Text = "Готово!";
                // Форма закроется из MainForm после завершения задачи
            }
        }

        public void ReportStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ReportStatus(message)));
                return;
            }
            lblStatus.Text = message;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
            btnCancel.Enabled = false;
            lblStatus.Text = "Отмена операции...";
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Разрешаем закрытие, если операция отменена ИЛИ завершена
            if (_cts.IsCancellationRequested == false &&
                DialogResult == DialogResult.None &&
                progressBar.Style == ProgressBarStyle.Marquee)
            {
                // Запрещаем закрывать крестиком ТОЛЬКО во время активной работы
                e.Cancel = true;
                return;
            }
            base.OnFormClosing(e);
        }

        public async Task CloseWithDelayAsync()
        {
            if (InvokeRequired)
            {
                await Invoke(new Func<Task>(CloseWithDelayAsync));
                return;
            }

            lblStatus.Text = "Готово!";
            progressBar.Value = 100;
            btnCancel.Enabled = false;

            await Task.Delay(1500); // Ждём 1.5 секунды
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
