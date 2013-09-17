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

        //[Obsolete("The With methods are obsolete and will be removed in version 2 of the AWS SDK for .NET. See http://aws.amazon.com/sdkfornet/#version2 for more information.")]
        //public GetFileAccessRequest WithBucketName(string bucketName)
        //{
        //    this.BucketName = bucketName;
        //    return this;
        //}

        //[Obsolete("The With methods are obsolete and will be removed in version 2 of the AWS SDK for .NET. See http://aws.amazon.com/sdkfornet/#version2 for more information.")]
        //public GetFileAccessRequest WithMarker(string marker)
        //{
        //    this.Marker = marker;
        //    return this;
        //}

        //[Obsolete("The With methods are obsolete and will be removed in version 2 of the AWS SDK for .NET. See http://aws.amazon.com/sdkfornet/#version2 for more information.")]
        //public GetFileAccessRequest WithMaxKeys(int maxKeys)
        //{
        //    this.MaxKeys = maxKeys;
        //    return this;
        //}
    }
}
