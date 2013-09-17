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

        public static bool IsTransitionState(FileAccessMode mode)
        {
            switch(mode)
            {
                case FileAccessMode.SwitchingToDisabled:
                case FileAccessMode.SwitchingToReadOnly:
                case FileAccessMode.SwitchingToReadWrite:
                    return true;
                default:
                    return false;
            }
        }

        public static bool TransitionsToState(FileAccessMode targetState, FileAccessMode currentState)
        {
            switch(targetState)
            {
                case FileAccessMode.ReadOnly:
                    return currentState == FileAccessMode.SwitchingToReadOnly;
                case FileAccessMode.ReadWrite:
                    return currentState == FileAccessMode.SwitchingToReadWrite;
                case FileAccessMode.Disabled:
                    return currentState == FileAccessMode.SwitchingToDisabled;
                default:
                    return false;
            }
        }
    }
}
