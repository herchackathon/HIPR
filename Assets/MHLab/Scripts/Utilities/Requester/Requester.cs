using System;
using System.Collections.Specialized;

namespace MHLab.Utilities
{
    public abstract class Requester : IRequester, IDisposable
    {
        public virtual void Dispose()
        {
            
        }

        public virtual Response Request(string endpoint, NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }
    }
}
