using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Danbooru_Checker
{
    public partial class DanbooruCheckerForm : Form
    {
        public DanbooruCheckerForm()
        {
            InitializeComponent();
            OpenDirectory(DanbooruChecker.Instance.Directory);
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (checking)
                return;

            DialogResult result = dialogFolderBrowser.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                OpenDirectory(dialogFolderBrowser.SelectedPath);
                DanbooruChecker.Instance.Save();
            }
        }

        private void buttonAuthenticate_Click(object sender, EventArgs e)
        {
            if (!checking)
                dialogApiKey.ShowDialog();
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            // Already checking, do nothing
            if (checking)
                return;

            // All files checked, do nothing
            if (active.TrueForAll(i => i.HasChecked))
            {
                Output("All files checked");
                return;
            }

            checking = true;

            List<Image> check = active.FindAll(i => !i.HasChecked);

            Task.Run(() =>
            {
                check.ForEach(i =>
                {
                    Output("Checking " + i.FileName);
                    i.Validate();
                });

                Invoke((MethodInvoker)delegate() 
                {
                    DanbooruChecker dan = DanbooruChecker.Instance;
                    dan.Cache(active);
                    dan.Save();

                    UpdateData(active);
                    Output("Finished");

                    checking = false;
                });
            });
        }

        private void dataImage_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Image image = active[e.RowIndex];

            // Show file in explorer
            if (e.ColumnIndex == 0)
            {
                Process.Start("explorer.exe", string.Format("/select,\"{0}\"", image.FilePath));
            }

            // Open URL (if exists)
            else if (e.ColumnIndex == 1)
            {
                Uri url = image.URL;
                if (url != null)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(url.ToString());
                    Process.Start(startInfo);
                }
            }
        }

        private void UpdateData(List<Image> images)
        {
            dataImage.Rows.Clear();
            images.ForEach(i => dataImage.Rows.Add(i.FileName, i.URL));
        }

        private void UpdateData(Image image)
        {
            // TODO update a single data
        }

        private void OpenDirectory(string path)
        {
            active = DanbooruChecker.Instance.OpenDirectory(path);
            labelDirectory.Text = path;

            UpdateData(active);

            if (path.Length > 0)
                Output("Opened directory: " + path);
        }

        private void Output(string text)
        {
            labelOutput.Text = text;
        }

        private bool checking = false;

        private ApiKeyDialog dialogApiKey = new ApiKeyDialog();
        private List<Image> active;
    }
}
