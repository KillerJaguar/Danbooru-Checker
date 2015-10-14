using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Danbooru_Checker
{
    [Serializable]
    public class Image
    {
        public Image(string filepath)
        {
            this.filepath = filepath;
        }

        public string FileName { get { return Path.GetFileName(filepath); } }
        public string FilePath { get { return Path.GetFullPath(filepath); } }
        public bool HasChecked { get { return hasChecked; } }
        public bool HasFound { get { return hasChecked && id != -1; } }

        public Uri URL
        {
            get
            {
                return HasFound ? new Uri("https://danbooru.donmai.us/posts/" + id) : null;
            }
        }

        public string Hash
        {
            get
            {
                // Read the file as bytes
                byte[] fileBytes = File.ReadAllBytes(FilePath);

                // Hash the bytes into MD5
                MD5 hash = MD5.Create();
                hash.ComputeHash(fileBytes);

                // Convert the MD5 into a string
                return BitConverter.ToString(hash.Hash).Replace("-", String.Empty);
            }
        }

        public void Validate()
        {
            // Do not validate again if already checked
            if (hasChecked)
                return;

            // Create the WebRequest to GET the XML data
            string url = "https://danbooru.donmai.us/posts.xml?tags=md5%3A" + Hash;
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.ContentType = "text/xml";
            request.Credentials = new NetworkCredential(
                (string) Properties.Settings.Default["Login"],
                (string) Properties.Settings.Default["ApiKey"]);
            request.Method = "GET";
            request.Proxy = null;

            // Get the response, blocking call
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Create the responding XML document
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                XmlElement posts = doc["posts"];

                // Get the ID (if an MD5 match was found)
                if (posts.HasChildNodes)
                    id = int.Parse(posts["post"]["id"].InnerText);
            }

            // Set the Image checked, so it does not check again
            hasChecked = true;
        }

        private string filepath;
        private bool hasChecked = false;
        private int id = -1;
    }

    public sealed class DanbooruChecker
    {
        public static DanbooruChecker Instance
        {
            get { return instance.Value; }
        }

        public string Directory
        {
            get { return Properties.Settings.Default.LastDir; }
            set { OpenDirectory(value); }
        }

        public string ApiKey
        {
            get { return Properties.Settings.Default.ApiKey; }
            set { Properties.Settings.Default.ApiKey = value; }
        }

        public string Login
        {
            get { return Properties.Settings.Default.Login; }
            set { Properties.Settings.Default.Login = value; }
        }

        public void Cache(List<Image> images)
        {
            foreach (Image image in images)
                if (image.URL != null && !cache.ContainsKey(image.FilePath))
                    cache[image.FilePath] = image;
        }

        public void Save()
        {
            Properties.Settings.Default.Save();

            System.IO.Directory.CreateDirectory(SaveDirectoryPath);

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(SaveFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                formatter.Serialize(stream, cache);
        }

        public List<Image> OpenDirectory(string path, SearchOption option = SearchOption.TopDirectoryOnly)
        {
            List<Image> images = new List<Image>();

            if (System.IO.Directory.Exists(path))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                foreach (FileInfo fileInfo in dirInfo.EnumerateFiles("*", option))
                {
                    string filename = fileInfo.Name;
                    string filepath = fileInfo.FullName;

                    bool canAdd = false;
                    foreach (string extension in ValidExtensions)
                        canAdd = canAdd || fileInfo.Extension == extension;

                    if (canAdd)
                    {
                        bool inCache = cache.ContainsKey(filepath);
                        images.Add(inCache ? cache[filepath] : new Image(filepath));
                    }
                }

                Properties.Settings.Default.LastDir = path;
            }

            return images;
        }

        public int CheckDanbooru(string hash)
        {
            // Create the WebRequest to GET the XML data
            string url = "https://danbooru.donmai.us/posts.xml?tags=md5%3A" + hash;
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.ContentType = "text/xml";
            request.Credentials = new NetworkCredential(Login, ApiKey);
            request.Method = "GET";
            request.Proxy = null;

            // Get the response, blocking call
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Create the responding XML document
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                XmlElement posts = doc["posts"];

                // Get the ID (if an MD5 match was found)
                if (posts.HasChildNodes)
                    return int.Parse(posts["post"]["id"].InnerText);
            }

            return -1;
        }

        private DanbooruChecker()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(SaveFilePath, FileMode.Open, FileAccess.Read))
                    cache = (Dictionary<string, Image>)formatter.Deserialize(stream);
            }
            catch (DirectoryNotFoundException)
            { }
            catch (FileNotFoundException)
            { }
        }

        private static readonly Lazy<DanbooruChecker> instance =
            new Lazy<DanbooruChecker>(() => new DanbooruChecker());

        private static readonly string SaveDirectoryPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Danbooru Checker");

        private static readonly string SaveFilePath = Path.Combine(
            SaveDirectoryPath, "data.bin");

        /// <summary>
        /// List of valid extensions to check on Danbooru.
        /// Only includes common image file extensions.
        /// </summary>
        private static readonly string[] ValidExtensions = 
            { ".png", ".jpg", ".jpeg", ".gif" };

        /// <summary>
        /// A dictionary of images already searched. Only images with a result
        /// are cached to disk.
        /// </summary>
        private Dictionary<string, Image> cache =
            new Dictionary<string, Image>();
    }

    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DanbooruCheckerForm());
        }
    }
}
