using MDIPaint;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
namespace MDIPaint
{    
    public partial class PluginManagerForm : Form
    {
        private readonly List<MainForm.PluginInfo> plugins;

        public PluginManagerForm(List<MainForm.PluginInfo> pluginList)
        {
            InitializeComponent();
            plugins = pluginList;
            LoadPlugins();
        }

        private void LoadPlugins()
        {
            foreach (var plugin in plugins)
            {
                int rowIndex = dgvPlugins.Rows.Add();
                DataGridViewRow row = dgvPlugins.Rows[rowIndex];

                row.Cells["Column1"].Value = plugin.Name;
                row.Cells["Column2"].Value = plugin.Author;
                row.Cells["Column3"].Value = plugin.Version;
                row.Cells["Column4"].Value = plugin.Enabled;
                // Сохраняем ссылку на плагин в строке
                row.Tag = plugin;
            }
        }

        private void PluginManagerForm_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Сохраняем изменения обратно в список
            for(int i = 0; i < dgvPlugins.Rows.Count; i++)
            {
                DataGridViewRow row = dgvPlugins.Rows[i];

                // Пропускаем новую пустую строку, если она есть
                if (row.IsNewRow) continue;

                if (row.Tag is MainForm.PluginInfo plugin)
                {
                    // Читаем значение чекбокса
                    bool enabled = Convert.ToBoolean(row.Cells["Column4"].Value ?? false);
                    plugin.Enabled = enabled;
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
