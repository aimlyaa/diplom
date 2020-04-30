using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace TesterClient
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new auth());
        }
    }

    class JsonAuth
    {
        public string group { get; set; }
        public string name { get; set; }
        
    }

    class JsonEndTest
    {
        public string testName { get; set; }

    }
    class JsonSendAnswer
    {
        public string testName { get; set; }
        public string questionString { get; set; }
        public int answerIndex { get; set; }
    }
    class JsonToken
    {
        public string token { get; set; }
    }
    public class JsonServerResult
    {
        public string result { get; set; }
    }
    public class JsonEndTestResult
    {
        public string result { get; set; }
    }
    public class RootObject
    {
        public List<TestDataList> testData { get; set; }
        public List<TestsList> testsList { get; set; }
        public string answers { get; set; }
        public int avaliableTime { get; set; }
        public int ended { get; set; }
    }

    public partial class TestsList
    {
        public string Name { get; set; }
    }

    public partial class TestDataList
    {
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }


    }

    class GlobalVars {
        public static string serverIp = "127.0.0.1";
        public static string serverPort = "8000";
        public static string clientToken;
        public static string testName;
        public static RootObject testData;
        public static int testLenght;
        public static int avaliableTime;
        public static List<int> userAnswers = new List<int>();
        public static int good;
    }
    class Requests
    {
        public static string testsList = null;
        public static string qst = null;
        public static string ans = null;
        public static List<int> usrAnsws = null;
        public static string POST(string request, string group, string name)
        {
            JsonAuth jsonSer = new JsonAuth();
            jsonSer.group = group;
            jsonSer.name = name;
            return SendRequest(request, jsonSer);
        }
        public static bool POST(string request, string actualQuestion, int selectedAnswer) {
            JsonSendAnswer jsonSer = new JsonSendAnswer();
            jsonSer.testName = GlobalVars.testName;
            jsonSer.questionString = actualQuestion;
            jsonSer.answerIndex = selectedAnswer;
            string result = SendRequest(request, jsonSer);
            if (result == "0")
            {
                return false;
            }
            else {
                return true;
            }
        }
        public static void POST(string request) {
            JsonEndTest jsonSer = new JsonEndTest();
            jsonSer.testName = GlobalVars.testName;
            SendRequest(request, jsonSer);
        }
        private static string SendRequest(string request, object jsonSer)
        {
            string data = JsonConvert.SerializeObject(jsonSer);
            byte[] byteArray = Encoding.UTF8.GetBytes(data);

            WebRequest re = WebRequest.Create("http://" + GlobalVars.serverIp + ":" + GlobalVars.serverPort + "/api/" + request + "/");
            re.Method = "POST";
            re.ContentType = "application/json";
            re.ContentLength = byteArray.Length;
            if (request == "send_answer" | request == "end_test" | request == "sync_time")
            {
                re.Headers["token"] = GlobalVars.clientToken;
            }

            Stream dataStream = re.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);

            WebResponse response;
            response = re.GetResponse();

            string jsonString;
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                jsonString = reader.ReadToEnd();
            }
            response.Close();
            switch (request)
            {
                default:
                    return "default";
                case "auth":
                    JsonToken jsonDeSer = JsonConvert.DeserializeObject<JsonToken>(jsonString);
                    return jsonDeSer.token;
                case "send_answer":
                    JsonServerResult serverAnswer = JsonConvert.DeserializeObject<JsonServerResult>(jsonString);
                    return serverAnswer.result;
                case "end_test":
                    JsonEndTestResult res = JsonConvert.DeserializeObject<JsonEndTestResult>(jsonString);
                    return res.result;
            }
        }

        public static string GET()
        {
            WebRequest request;
            request = WebRequest.Create("http://" + GlobalVars.serverIp + ":" + GlobalVars.serverPort + "/api/tests_list");
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers["token"] = GlobalVars.clientToken;

            WebResponse responce;
            responce = request.GetResponse();

            StreamReader sr;
            using (Stream stream = responce.GetResponseStream()) {
                sr = new StreamReader(stream);
                string jsonString = sr.ReadToEnd();
                byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonString);
                RootObject jsonDeSer = JsonConvert.DeserializeObject<RootObject>(jsonString);
                sr.Close();
                for (int i = 0; i < jsonDeSer.testsList.Count; i++)
                {
                    testsList = testsList + jsonDeSer.testsList[i].Name + ",";
                }
                return testsList;
            }
        }
        public static void GET(string testName) {
            WebRequest request;
            request = WebRequest.Create("http://" + GlobalVars.serverIp + ":" + GlobalVars.serverPort + "/api/get_test/" + "?test=" + testName);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers["token"] = GlobalVars.clientToken;

            WebResponse responce;
            responce = request.GetResponse();

            StreamReader sr;
            using (Stream stream = responce.GetResponseStream())
            {
                sr = new StreamReader(stream);
                string jsonString = sr.ReadToEnd();
                byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonString);
                GlobalVars.testData = JsonConvert.DeserializeObject<RootObject>(jsonString);
                GlobalVars.avaliableTime = GlobalVars.testData.avaliableTime;
                foreach (string ans in GlobalVars.testData.answers.Split(',')) {
                    if (ans == "1"){
                        GlobalVars.good++;
                    }
                    GlobalVars.userAnswers.Add(int.Parse(ans));
                    GlobalVars.testLenght++;
                }
                sr.Close();
            }
        }
    }
}
