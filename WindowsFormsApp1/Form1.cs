using Newtonsoft.Json;
using System;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string res = SendNotification("app registration token key string 152 bytes here", textBox2.Text);
        }


        public string SendNotification(string deviceId, string message)
        {
            string SERVER_API_KEY = "AAAA3EnShoY:APA91bEvDbFQJVKzsZQ1Q4LTnCiJ6juOZRH66mrB3wwk6vPUyNDA4IauSeTsiLWBGv_ZcgscVKIX8ckW6OZRcdwXiGPndDVi5TUUuiwarwU9MvrXCkV6kEB_yYSJfkKkZbZwa3JPbPV9";

            var value = message;
            string resultStr = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8;";
            request.Headers.Add(string.Format("Authorization: key={0}", SERVER_API_KEY));

            var postData =
            new
            {
                data = new
                {
                    title = textBox1.Text,
                    body = message,
                },

                // FCM allows 1000 connections in parallel.
                to = deviceId
            };

            //Linq to json
            string contentMsg = JsonConvert.SerializeObject(postData);
            Debug.WriteLine("contentMsg = " + contentMsg);

            Byte[] byteArray = Encoding.UTF8.GetBytes(contentMsg);
            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                resultStr = reader.ReadToEnd();
                Debug.WriteLine("response: " + resultStr);
                reader.Close();
                responseStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                resultStr = "";
                Debug.WriteLine(e.Message);
            }

            return resultStr;

        }

        
    }
}
