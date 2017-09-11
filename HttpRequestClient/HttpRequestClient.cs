using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequest
{
    public class HttpRequestClient
    {
        private readonly string _serverAddress;

        public static string HttpMethodPost => "POST";

        public static string HttpMethodGet => "GET";

        public HttpRequestClient(string serverAddress)
        {
            _serverAddress = serverAddress;
        }

        public void StartRequest(string api, string method, XHttpRequestParamters paramter, HttpResponseHandler handler)
        {
            var request = (HttpWebRequest)WebRequest.Create($"{_serverAddress}/{api}");
            request.Method = method;
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            foreach (var headerString in paramter.HeaderStrings)
            {
                request.Headers[headerString.Key] = headerString.Value;
            }

            var builder = new StringBuilder();
            foreach (var bodyParamter in paramter.BodyParamters)
            {
                builder.AppendFormat("&{0}={1}", bodyParamter.Key, bodyParamter.Value);
            }
            if (builder.Length > 0)
            {
                builder.Remove(0, 1);
            }

            if (method == HttpMethodPost)
            {
                request.BeginGetRequestStream(PostCallBack, new HttpRequestAsyncState(request, builder, handler));
            }
            if (method == HttpMethodGet)
            {
                request.BeginGetResponse(ReadCallBack, new HttpResponseAsyncResult(request, handler));
            }
        }

        public Task<string> StartRequestAsync(string api, string method, XHttpRequestParamters paramter)
        {
            var request = (HttpWebRequest)WebRequest.Create($"{_serverAddress}/{api}");
            request.Method = method;
            request.Accept = "application/json";
            request.ContentType = "application/x-www-form-urlencoded";
            foreach (var headerString in paramter.HeaderStrings)
            {
                request.Headers[headerString.Key] = headerString.Value;
            }

            var builder = new StringBuilder();
            foreach (var bodyParamter in paramter.BodyParamters)
            {
                builder.AppendFormat("&{0}={1}", bodyParamter.Key, bodyParamter.Value);
            }
            if (builder.Length > 0)
            {
                builder.Remove(0, 1);
            }

            if (method == HttpMethodPost)
            {
                var postStream = request.GetRequestStream();
                var byteArray = Encoding.UTF8.GetBytes(builder.ToString());
                postStream.Write(byteArray, 0, byteArray.Length);
            }

            var task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        private void PostCallBack(IAsyncResult asynchronousResult)
        {
            var asyncResult = (HttpRequestAsyncState)asynchronousResult.AsyncState;
            try
            {
                var postStream = asyncResult.Request.EndGetRequestStream(asynchronousResult);
                var byteArray = Encoding.UTF8.GetBytes(asyncResult.BodyParamters.ToString());
                postStream.Write(byteArray, 0, byteArray.Length);

                asyncResult.Request.BeginGetResponse(ReadCallBack, new HttpResponseAsyncResult(asyncResult.Request, asyncResult.Handler));
            }
            catch (Exception ex)
            {
                asyncResult.Handler.Error(ex);
            }
        }

        private void ReadCallBack(IAsyncResult asynchronousResult)
        {
            var asyncResult = (HttpResponseAsyncResult)asynchronousResult.AsyncState;
            try
            {
                var reponse = asyncResult.Request.EndGetResponse(asynchronousResult);
                var stream = reponse.GetResponseStream();
                if (stream == null)
                {
                    asyncResult.Handler.Response(string.Empty);
                    return;
                }
                using (var reader = new StreamReader(stream))
                {
                    var responseStr = reader.ReadToEnd();
                    asyncResult.Handler.Response(responseStr);
                }
            }
            catch (Exception ex)
            {
                asyncResult.Handler.Error(ex);
            }
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null) return string.Empty;
                using (var sr = new StreamReader(responseStream))
                {
                    var strContent = sr.ReadToEnd();
                    return strContent;
                }
            }
        }
    }
}
