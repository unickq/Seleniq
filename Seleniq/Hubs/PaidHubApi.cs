using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Seleniq.Hubs.CrossBrowserTesting;

namespace Seleniq.Hubs
{
    public class PaidHubApi
    {
        private readonly HubType _type;
        private readonly Uri _uri;
        private readonly string _key;
        private readonly string _secret;
        private readonly string _session;

        public PaidHubApi(string key, string secret, Uri uri, string session, HubType type)
        {
            _key = key;
            _secret = secret;
            _uri = uri;
            _session = session;
            _type = type;
        }

        public void UpdateTestResult(bool passed, [Optional] string message)
        {
            switch (_type)
            {
                case HubType.Testingbot:
                    var request = (HttpWebRequest) WebRequest.Create(_uri);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "PUT";
                    var usernamePassword = _key + ":" + _secret;
                    var mycache = new CredentialCache
                    {
                        {
                            _uri, "Basic",
                            new NetworkCredential(_key, _secret)
                        }
                    };
                    request.Credentials = mycache;
                    request.Headers.Add("Authorization",
                        "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));

                    using (var writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(
                            "test[success]=" + (passed ? "1" : "0") +
                            "&test[status_message]=" + message);
                    }
                    var response = request.GetResponse();
                    response.Close();
                    break;
                case HubType.Browserstack:
                    var resultStr = "failed";
                    if (passed) resultStr = "passed";                   
                    try
                    {
                        PutJson($"{{\"status\":\"{resultStr}\", \"reason\":\"{message}\"}}");
                    }
                    catch (Exception)
                    {
                        PutJson($"{{\"status\":\"{resultStr}\", \"reason\":\"ToDo\"}}");
                    }
                    break;
                case HubType.CrossbrowserTesting:
                    var result = "fail";
                    if (passed) result = "pass";
                    var cbtApi = new CbtApi(_uri.AbsoluteUri, _key, _secret);
                    cbtApi.SetScore(_session, result);
                    cbtApi.SetDescription(_session, cbtApi.TakeSnapshot(_session), message);
                    break;
                case HubType.Saucelabs:
                    PutJson("{\"passed\":" + passed.ToString().ToLower() + "}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private void PutJson(string reqString)
        {
            var requestData = Encoding.UTF8.GetBytes(reqString);
            var myWebRequest = WebRequest.Create(_uri);
            var myHttpWebRequest = (HttpWebRequest) myWebRequest;
            myWebRequest.ContentType = "application/json";
            myWebRequest.Method = "PUT";
            myWebRequest.ContentLength = requestData.Length;
            using (var st = myWebRequest.GetRequestStream())
            {
                st.Write(requestData, 0, requestData.Length);
            }
            var networkCredential = new NetworkCredential(_key, _secret);
            var myCredentialCache = new CredentialCache {{_uri, "Basic", networkCredential}};
            myHttpWebRequest.PreAuthenticate = true;
            myHttpWebRequest.Credentials = myCredentialCache;
            myWebRequest.GetResponse().Close();
        }
    }
}