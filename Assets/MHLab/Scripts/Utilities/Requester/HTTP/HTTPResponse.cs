using System.Net;
using System.Text;

namespace MHLab.Utilities.HTTP
{
    public class HTTPResponse : Response
    {
        /// <summary>
        /// Returns the data in UTF8 string format. Useful for text encodings like JSON.
        /// </summary>
        public string Data
        {
            get
            {
                if (RawData == null)
                    return string.Empty;
                else
                    return Encoding.UTF8.GetString(RawData);
            }
        }

        public int Status { get; set; }

        public HTTPResponse(byte[] data, WebExceptionStatus status) : base(data)
        {
            Status = (status == WebExceptionStatus.Success) ? 200 : 403;
        }
    }
}
