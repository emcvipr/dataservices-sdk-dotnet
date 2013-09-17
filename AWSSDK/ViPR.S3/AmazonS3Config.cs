using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.S3
{
    public partial class AmazonS3Config
    {
        private string ns;
        
        /// <summary>
        /// Gets or sets the ViPR namespace to access.
        /// </summary>
        public string Namespace
        {
            get { return this.ns; }
            set { this.ns = value; }
        }

        [Obsolete("The With methods are obsolete and will be removed in version 2 of the AWS SDK for .NET. See http://aws.amazon.com/sdkfornet/#version2 for more information.")]
        public AmazonS3Config WithNamespace(string ns)
        {
            this.Namespace = ns;
            return this;
        }
    }
}
