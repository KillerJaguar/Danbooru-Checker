using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Danbooru_Checker
{
    public partial class DanbooruCheckerForm : Form
    {
        public DanbooruCheckerForm()
        {
            InitializeComponent();
            dialogApiKey = new ApiKeyDialog();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            DialogResult result = dialogFolderBrowser.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                // Get the path to the selected directory
                string path = dialogFolderBrowser.SelectedPath;
                labelDirectory.Text = path;

                // Save the path
                Properties.Settings.Default["LastDir"] = path;
                Properties.Settings.Default.Save();

                // Open the directory
                OpenDirectory(path);

                // Update the displayed data
                UpdateData();
            }
        }

        private void buttonAuthenticate_Click(object sender, EventArgs e)
        {
            dialogApiKey.ShowDialog();
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            if (images != null)
            {
                try
                {
                    foreach (Image image in images)
                        image.Validate();
                }
                catch (System.Net.WebException)
                {
                    // 421 User Throttled: User is throttled, try again later
                }
                finally
                {
                    UpdateData();
                }
            }
        }

        private void DanbooruCheckerForm_Load(object sender, EventArgs e)
        {
            string path = (string) Properties.Settings.Default["LastDir"];
            if (path != null && path.Length > 0)
            {
                labelDirectory.Text = path;
                OpenDirectory(path);
                UpdateData();
            }
        }

        private void OpenDirectory(string path)
        {
            // Create the list of images 
            images = new List<Image>();

            // Iterate through each file, adding onto the image list
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            foreach (FileInfo fileInfo in dirInfo.EnumerateFiles())
                images.Add(new Image(fileInfo.FullName));
        }

        private void UpdateData()
        {
            dataImage.Rows.Clear();
            foreach (Image image in images)
                dataImage.Rows.Add(image.FilePath, image.URL);
        }

        private ApiKeyDialog dialogApiKey;
        private List<Image> images;
    }
}
