using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Amazon.S3.Model
{
    public class GetBucketFileAccessModeResponse : BucketFileAccessModeResponseBase
    {
    }

    public class SetBucketFileAccessModeResponse : BucketFileAccessModeResponseBase
    {
    }

    public class BucketFileAccessModeResponseBase : S3Response
    {
        [XmlElement("AccessMode")]
        public FileAccessMode AccessMode { get; set; }

        [XmlElement("Duration")]
        public long Duration { get; set; }

        [XmlArray("HostList")]
        [XmlArrayItem("Host")]
        public List<string> HostList { get; set; }

        [XmlElement("Uid")]
        public string Uid { get; set; }

        [XmlElement("StartToken")]
        public string StartToken { get; set; }

        [XmlElement("EndToken")]
        public string EndToken { get; set; }

    }
}
