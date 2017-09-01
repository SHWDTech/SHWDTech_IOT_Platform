using System;

namespace HttpRequest
{
    public class HttpRerquestEventArgs : EventArgs
    {
        public string Response { get; set; }

        public string Error { get; set; }

        public Exception Exception { get; set; }
    }
}
