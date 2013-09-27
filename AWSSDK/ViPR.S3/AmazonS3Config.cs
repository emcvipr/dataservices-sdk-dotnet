/*
 * Copyright 2013 EMC Corporation. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0.txt
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
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
