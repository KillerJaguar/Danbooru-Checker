using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Danbooru_Checker
{
    public partial class DanbooruCheckerForm : Form
    {
        public DanbooruCheckerForm()
        {
            InitializeComponent();
            dialogApiKey = new ApiKeyDialog();

            DanbooruChecker dan = DanbooruChecker.Instance;
            active = dan.OpenDirectory(dan.Directory);

            labelDirectory.Text = dan.Directory;

            UpdateData(active);
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            DialogResult result = dialogFolderBrowser.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                DanbooruChecker dan = DanbooruChecker.Instance;

                // Get the path to the selected directory
                string path = dialogFolderBrowser.SelectedPath;
                labelDirectory.Text = path;

                // Open the directory
                active = dan.OpenDirectory(path);

                // Update the displayed data
                UpdateData(active);

                dan.Save();
            }
        }

        private void buttonAuthenticate_Click(object sender, EventArgs e)
        {
            dialogApiKey.ShowDialog();
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            if (active != null)
            {
                try
                {
                    // TODO use tasks to speed up process
                    foreach (Image image in active)
                        image.Validate();
                }
                catch (System.Net.WebException)
                {
                    // 421 User Throttled: User is throttled, try again later
                    // TODO handle error
                }
                finally
                {
                    DanbooruChecker dan = DanbooruChecker.Instance;
                    dan.Cache(active);
                    dan.Save();

                    UpdateData(active);
                }
            }
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
            foreach (Image image in images)
                dataImage.Rows.Add(image.FileName, image.URL);
        }

        private ApiKeyDialog dialogApiKey;
        private List<Image> active;
    }
}
