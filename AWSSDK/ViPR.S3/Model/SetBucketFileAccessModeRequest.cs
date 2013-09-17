
using System;
using System.Linq;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Amazon.S3.Model
{
    public class SetBucketFileAccessModeRequest : S3Request
    {
        private static readonly  FileAccessMode[]  ALLOWED_ACCESS_MODES = {
               FileAccessMode.Disabled, FileAccessMode.ReadOnly, FileAccessMode.ReadWrite};
   
        private FileAccessMode? accessMode;

        [XmlElement("AccessMode")]
        public FileAccessMode? AccessMode
        {
            get { return this.accessMode; }

            set
            {
                if (value == null)
                {
                    this.accessMode = null;
                    return;
                }
                if (!ALLOWED_ACCESS_MODES.Contains((FileAccessMode)value))
                    throw new ArgumentException("Access mode must be one of " + 
                        string.Join(",", ALLOWED_ACCESS_MODES.Select(m => m.ToString()).ToArray()));
                this.accessMode = value;
            }
        }

        [XmlElement("Duration")]
        public long Duration { get; set; }

        [XmlArray("HostList")]
        [XmlArrayItem("Host")]
        public IList<string> HostList { get; set; }

        [XmlElement("Uid")]
        public string Uid { get; set; }

        [XmlElement("Token")]
        public string Token { get; set; }

        [XmlElement("BucketName")]
        public string BucketName { get; set; }

    }
}
