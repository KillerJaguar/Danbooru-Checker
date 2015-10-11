using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Danbooru_Checker
{
    public partial class ApiKeyDialog : Form
    {
        public ApiKeyDialog()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Login"]  = textLogin.Text;
            Properties.Settings.Default["ApiKey"] = textApiKey.Text;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textApiKey.Text = textLogin.Text = "";
        }

        private void labelApiKey_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo((string) labelApiKey.Tag);
            Process.Start(startInfo);
        }

        private void ApiKeyDialog_Shown(object sender, EventArgs e)
        {
            textLogin.Text  = (string) Properties.Settings.Default["Login"];
            textApiKey.Text = (string) Properties.Settings.Default["ApiKey"];
        }
    }
}
