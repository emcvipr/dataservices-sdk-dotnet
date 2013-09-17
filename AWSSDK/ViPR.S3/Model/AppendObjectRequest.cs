
using System;
using System.Xml.Serialization;
using System.Collections.Specialized;
using Amazon.Util;

namespace Amazon.S3.Model
{
    /// <summary>
    /// </summary>
    public class AppendObjectRequest : UpdateObjectRequest
    {
        private static readonly Tuple<long, long?> UPDATE_RANGE = new Tuple<long, long?>(-1, null);

        public AppendObjectRequest()
        {
            this.UpdateRange = UPDATE_RANGE;
        }
    }

}
