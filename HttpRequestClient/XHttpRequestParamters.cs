using System.Collections.Generic;

namespace HttpRequest
{
    public class XHttpRequestParamters
    {
        public Dictionary<string, string> HeaderStrings { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> BodyParamters { get; } = new Dictionary<string, string>();

        public void AddHeader(string key, string value)
        {
            HeaderStrings[key] = value;
        }

        public void AddBodyParamter(string key, string value)
        {
            BodyParamters[key] = value;
        }
    }
}
