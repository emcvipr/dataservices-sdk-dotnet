using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Amazon.S3.Model
{
    public class GetFileAccessRequest : S3Request
    {
        [XmlElement("BucketName")]
        public string BucketName { get; set; }

        [XmlElement("Marker")]
        public string Marker { get; set; }

        [XmlElement("MaxKeys")]
        public int MaxKeys { get; set; }
    }
}
