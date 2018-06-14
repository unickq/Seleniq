using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Seleniq.Hubs.CrossBrowserTesting
{
    public class CbtApi
    {
        private readonly string _cbtUser;
        private readonly string _cbtKey;
        private readonly string _cbtApiUrl;

        public CbtApi(string cbtApiUrl, string cbtUser, string cbtKey)
        {
            _cbtUser = cbtUser;
            _cbtKey = cbtKey;
            _cbtApiUrl = cbtApiUrl;
        }

        private HttpWebRequest GetCommonRequest(string url, string method)
        {
            //            Console.WriteLine(url);
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = method;
            request.Credentials = new NetworkCredential(_cbtUser, _cbtKey);
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "HttpWebRequest";
            return request;
        }

        public string TakeSnapshot(string sessionId)
        {
            var request = GetCommonRequest(_cbtApiUrl + "/" + sessionId + "/snapshots", "POST");
            var response = (HttpWebResponse) request.GetResponse();
            var stream = response.GetResponseStream();
            if (stream == null) return null;
            var responseString = new StreamReader(stream).ReadToEnd();
            var myregex = new Regex("(?<=\"hash\": \")((\\w|\\d)*)");
            var snapshotHash = myregex.Match(responseString).Value;
            //            Console.WriteLine(snapshotHash);
            request.GetResponse().Close();
            return snapshotHash;
        }

        public void SetDescription(string sessionId, string snapshotHash, string description)
        {
            var encoding = new ASCIIEncoding();
            var putData = encoding.GetBytes("description=" + description);
            // create the request
            var request = GetCommonRequest(_cbtApiUrl + "/" + sessionId + "/snapshots/" + snapshotHash, "PUT");
            // write data to stream
            var stream = request.GetRequestStream();
            stream.Write(putData, 0, putData.Length);
            stream.Close();
            request.GetResponse().Close();
        }

        public void SetScore(string sessionId, string score)
        {
            var encoding = new ASCIIEncoding();
            var data = "action=set_score&score=" + score;
            var putdata = encoding.GetBytes(data);
            var request = GetCommonRequest(_cbtApiUrl + "/" + sessionId, "PUT");
            var newStream = request.GetRequestStream();
            newStream.Write(putdata, 0, putdata.Length);
            request.GetResponse().Close();
            newStream.Close();
        }
    }
}