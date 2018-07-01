using System;
using System.Collections.Specialized;

namespace MHLab.Utilities
{
    public interface IRequester
    {
        Response Request(String endpoint, NameValueCollection parameters);
    }
}
