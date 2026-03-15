using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MDIPaint
{
    public partial class UserFilterForm : Form
    {
        public UserFilterForm()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                string code = textBoxCode.Text;

                IPlugin plugin = DynamicPluginCompiler.Compile(code);

                if (Owner is MainForm main &&
                    main.ActiveMdiChild is DocumentForm doc)
                {
                    var context = new PluginContext
                    {
                        Image = (Bitmap)doc.Image.Clone(),
                        FilePath = doc.FilePath
                    };

                    plugin.Transform(context);

                    doc.Image = context.Image;
                    doc.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
