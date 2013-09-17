using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Amazon.S3.Model
{
    public class GetFileAccessResponse : S3Response
    {
        /// <summary>
        /// Gets or sets the mount points associated with the file access session.
        /// </summary>
        /// <remarks>
        /// Returns all of the mount points providing NFS access to the objects in a
        /// discrete list. These are provided as a convenience so that clients can
        /// start mount operations before the entire list of objects is received.
        /// Many objects may be hosted on a particular mount point.
        /// </remarks>
        [XmlArray("MountPoints")]
        [XmlArrayItem("MountPoint")]
        public List<string> MountPoints { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the list of objects has been truncated based on the maxKeys parameter.
        /// </summary>
        [XmlElement("IsTruncated")]
        public bool IsTruncated { get; set; }

        /// <summary>
        /// Gets or sets the NFS details for all the objects accessible via NFS.
        /// </summary>
        [XmlArray("Objects")]
        [XmlArrayItem("Object")]
        public List<FileAccessObject> Objects { get; set; }

        /// <summary>
        /// Gets or sets the last key returned by this fileaccess response. 
        /// </summary>
        /// <remarks>
        /// If populated, the results in this response are truncated.
        /// </remarks>
        [XmlElement("LastKey")]
        public string LastKey { get; set; }
    }

    public class FileAccessObject
    {
        /// <summary>
        /// Gets or sets the internal name given to the object.
        /// </summary>
        /// <remarks>
        /// This is not necessarily the key, but is assumed to be unique.
        /// </remarks>
        [XmlElement("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the mount point on which the object is located.
        /// </summary>
        [XmlElement("DeviceExport")]
        public string DeviceExport { get; set; }

        /// <summary>
        /// Gets or sets the path to the object relative to its mount point.
        /// </summary>
        [XmlElement("RelativePath")]
        public string RelativePath { get; set; }

        /// <summary>
        /// Gets or sets the UID of the Unix owner of the file representing the object.
        /// </summary>
        [XmlElement("Owner")]
        public string Owner { get; set; }
    }
}
