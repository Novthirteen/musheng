using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Net;

namespace WindowsService1
{
    public partial class BingPicDownload : ServiceBase
    {
        public BingPicDownload()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log("Inf:Service Started");
            GetBingImg(null, null);

            double sleeptime = 1000 * 60 * 60 * 24;//时间间隔
            System.Timers.Timer t = new System.Timers.Timer(sleeptime);//实例化Timer类; 
            t.Elapsed += new System.Timers.ElapsedEventHandler(GetBingImg);//到达时间的时候执行事件; 
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true);
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件;
        }

        protected override void OnStop()
        {
            Log("Inf:Service Stopped");
        }

        private void GetBingImg(object source, System.Timers.ElapsedEventArgs e)
        {
            string DownloadPicsDir = "D:\\wwwroot\\LoginPic\\";
            if (DateTime.Now.Hour <= 20 && DateTime.Now.Hour >= 8)
            {
                try
                {

                    string todayFile = DateTime.Now.ToString("yyyy-MM-dd") + ".jpg";
                    string usFilePath = DownloadPicsDir + todayFile;

                    if (!System.IO.Directory.Exists(DownloadPicsDir))
                        System.IO.Directory.CreateDirectory(DownloadPicsDir);

                    string bingFilePath;
                    if (!System.IO.File.Exists(usFilePath))
                    {
                        bingFilePath = GetBingImageFile("http://www.bing.com/?mkt=en-US");
                        if (!SavePhotoFromUrl(bingFilePath, usFilePath))
                        {
                            Log("Err:Download Pic Failure:" + todayFile);
                        }
                        else
                        {
                            Log("Inf:Download Pic Success:" + todayFile);
                        }
                    }
                    else
                    {
                        Log("Inf:Pictures  already exist:" + todayFile);
                    }
                }
                catch (Exception ex)
                {
                    Log("Err:System Error:" + ex.Message);
                }
            }
        }

        private string GetBingImageFile(string from)
        {
            //string websiteAddress = "http://www.bing.com";
            string fullWebpageText = RetrieveWebPage(from);
            string findStr = "g_img={url:'";
            int startIndex = fullWebpageText.IndexOf(findStr);
            startIndex += findStr.Length;
            int lastIndex = fullWebpageText.IndexOf('\'', startIndex);

            string imgPath = fullWebpageText.Substring(startIndex, lastIndex - startIndex);
            imgPath = imgPath.Replace(@"\", "");

            return "http://www.bing.com" + imgPath;
        }

        private string RetrieveWebPage(string query)
        {
            // prepare the web page we will be asking for
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(query);

            // execute the request
            System.Net.HttpWebResponse response = null;
            try
            {
                response = (System.Net.HttpWebResponse)request.GetResponse();
            }
            catch (Exception /*e*/)
            {
                return null;
            }

            string fileContent = string.Empty;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
            {
                fileContent = sr.ReadToEnd();
            }

            return fileContent;
        }

        private bool SavePhotoFromUrl(string Url, string FileName)
        {
            bool Value = false;
            WebResponse response = null;
            Stream stream = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                response = request.GetResponse();
                stream = response.GetResponseStream();

                Value = SaveBinaryFile(response, FileName);
            }
            catch (Exception err)
            {
                string aa = err.ToString();
            }
            return Value;
        }

        private bool SaveBinaryFile(WebResponse response, string FileName)
        {
            bool Value = true;
            byte[] buffer = new byte[1024];

            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }
            return Value;
        }

        private void Log(string logstr)
        {
            FileStream fs = new FileStream("D:\\wwwroot\\LoginPic\\BingPicDownloadLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine(logstr + " " + DateTime.Now.ToString() + "\n");
            m_streamWriter.Flush();
            m_streamWriter.Close();
            fs.Close();
        }
    }
}
