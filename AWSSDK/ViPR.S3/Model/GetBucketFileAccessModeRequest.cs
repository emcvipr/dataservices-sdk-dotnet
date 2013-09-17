using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Amazon.S3.Model
{
    public class GetBucketFileAccessModeRequest : S3Request
    {
        [XmlElement("BucketName")]
        public string BucketName { get; set; }
    }
}
