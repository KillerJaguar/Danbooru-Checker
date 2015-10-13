﻿using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

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

    public class DanbooruChecker
    {

    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DanbooruCheckerForm());
        }
    }
}
