using System;
using System.Collections.Specialized;
using System.Net;

namespace MHLab.Utilities.HTTP
{
    /// <summary>
    /// Represents an HTTP request.
    /// </summary>
    public class HTTPRequester : Requester
    {
        private string m_method = "GET";

        /// <summary>
        /// Get and set the request's method. It can be POST or GET.
        /// </summary>
        public string Method
        {
            get
            {
                return m_method;
            }
            set
            {
                if (value == "POST")
                    m_method = value;
                else
                    m_method = "GET";
            }
        }

        /// <summary>
        /// Performs a synchronous request on passed endpoint and returns an HTTPResponse.
        /// </summary>
        /// <param name="endpoint">The endpoint</param>
        /// <param name="parameters">Parameters for this request</param>
        /// <returns>The HTTPResponse</returns>
        public override Response Request(string endpoint, NameValueCollection parameters)
        {
            return Request(endpoint, parameters, null);
        }

        /// <summary>
        /// Performs a synchronous request on passed endpoint and returns an HTTPResponse.
        /// </summary>
        /// <param name="endpoint">The endpoint</param>
        /// <param name="parameters">Parameters for this request</param>
        /// <param name="header">Header for this request</param>
        /// <returns>The HTTPResponse</returns>
        public Response Request(string endpoint, NameValueCollection parameters, NameValueCollection header)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    if (header != null) client.Headers.Add(header);
                    if(Method == "POST")
                        return new HTTPResponse(client.UploadValues(endpoint, Method, parameters), WebExceptionStatus.Success);
                    else
                        return new HTTPResponse(client.DownloadData(endpoint), WebExceptionStatus.Success);
                }
                catch(WebException we)
                {
                    return new HTTPResponse(new byte[0], we.Status);
                }
            }
        }
    }
}
