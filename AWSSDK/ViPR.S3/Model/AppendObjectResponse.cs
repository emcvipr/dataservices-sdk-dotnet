

using System;

using Amazon.S3.Util;
using Amazon.Util;
using System.Xml.Serialization;

namespace Amazon.S3.Model
{
    /// <summary>
    /// The AppendObjectResponse contains any headers returned by S3.
    /// </summary>
    public class AppendObjectResponse : UpdateObjectResponse
    {
        /// <summary>
        /// Gets or sets the starting offset inside the object where the data was appended.
        /// </summary>
        [XmlElementAttribute(ElementName = "AppendOffset")]
        public long AppendOffset { get; set; }
    }
}