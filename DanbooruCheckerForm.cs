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

                SaveData();
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
                    SaveData();
                    UpdateData();
                }
            }
        }

        private void dataImage_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Image image = images[e.RowIndex];

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

        private void DanbooruCheckerForm_Load(object sender, EventArgs e)
        {
            string path = (string) Properties.Settings.Default["LastDir"];
            if (path != null && path.Length > 0)
            {
                labelDirectory.Text = path;
                if (!LoadData())
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
                dataImage.Rows.Add(image.FileName, image.URL);
        }

        private void SaveData()
        {
            Directory.CreateDirectory(SaveDirectoryPath);

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(SaveFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                formatter.Serialize(stream, images);
            }
        }

        private bool LoadData()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(SaveFilePath, FileMode.Open, FileAccess.Read))
                    images = (List<Image>)formatter.Deserialize(stream);
                return true;
            }
            catch (DirectoryNotFoundException)
            { }
            catch (FileNotFoundException)
            { }

            return false;
        }

        private ApiKeyDialog dialogApiKey;
        private List<Image> images;

        private static readonly string SaveDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Danbooru Checker");

        private static readonly string SaveFilePath = Path.Combine(
            SaveDirectoryPath, "images.bin");
    }
}
