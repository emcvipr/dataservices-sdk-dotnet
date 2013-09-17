using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Amazon.S3.Model
{
    public class UpdateObjectRequest : PutObjectRequest
    {
        [XmlElementAttribute(ElementName = "UpdateRange")]
        public Tuple<long, long?> UpdateRange { get; set; }

        [Obsolete("The With methods are obsolete and will be removed in version 2 of the AWS SDK for .NET. See http://aws.amazon.com/sdkfornet/#version2 for more information.")]
        public UpdateObjectRequest WithUpdateRange(long start, long end)
        {
            this.UpdateRange = new Tuple<long,long?>(start, end);
            return this;
        }

        [Obsolete("The With methods are obsolete and will be removed in version 2 of the AWS SDK for .NET. See http://aws.amazon.com/sdkfornet/#version2 for more information.")]
        public UpdateObjectRequest WithUpdateRange(Tuple<long, long?> updateRange)
        {
            this.UpdateRange = updateRange;
            return this;
        }

        [Obsolete("The With methods are obsolete and will be removed in version 2 of the AWS SDK for .NET. See http://aws.amazon.com/sdkfornet/#version2 for more information.")]
        public UpdateObjectRequest WithUpdateOffset(long startOffset)
        {
            this.UpdateRange = new Tuple<long, long?>(startOffset, null);
            return this;
        }

        //internal Tuple<long?, long?> ConvertUpdateRange()
        //{
        //    var range = new Tuple<long?, long?>();
        //    if (this.UpdateRange != null)
        //    {
        //        var split = this.UpdateRange.Split('-');
        //        if (split[0].Length > 0) range.First = long.Parse(split[0]);
        //        if (split[1].Length > 0) range.Second = long.Parse(split[1]);
        //    }
        //    return range;
        //}
    }
}
