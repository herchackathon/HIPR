using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MHLab.Utilities
{
    public class Response : IResponse
    {
        /// <summary>
        /// Represents the raw data of this response.
        /// </summary>
        public byte[] RawData;

        public Response(byte[] data)
        {
            RawData = data;
        }
    }
}
