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

                // Create the list of images 
                images = new List<Image>();

                // Iterate through each file, adding onto the image list
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                foreach (FileInfo fileInfo in dirInfo.EnumerateFiles())
                    images.Add(new Image(fileInfo.FullName));

                // Save the path
                Properties.Settings.Default["LastDir"] = path;
                Properties.Settings.Default.Save();

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
                catch (System.Net.WebException e)
                {
                    // 421 User Throttled: User is throttled, try again later
                }
                finally
                {
                    UpdateData();
                }
            }
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
