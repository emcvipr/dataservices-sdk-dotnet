using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;
using System.Configuration;

using Amazon.S3;
using Amazon.Runtime;
using Amazon.S3.Model;

namespace AWSSDK.Test
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void TestConnectivity1()
        {
            using (var client = CreateClient())
            {
                var buckets = client.ListBuckets();
                foreach (var bucket in buckets.Buckets)
                {
                    Debug.WriteLine(string.Format("Bucket: {0}", bucket.BucketName));
                }
            }
        }


        internal static AmazonS3Client CreateClient(AmazonS3Config config = null)
        {
            if(config == null) config = new AmazonS3Config();

            Uri endpoint = new Uri(ConfigurationManager.AppSettings["AWSServiceURL"]);
            if (endpoint.HostNameType == UriHostNameType.IPv4 || endpoint.HostNameType == UriHostNameType.IPv6)
            {
                // By default, the AWS .NET SDK does not support 
                // path-style buckets and nonstandard ports in the service
                // URL.  Therefore, we need to configure the ViPR endpoint
                // as a proxy instead of the ServiceURL since most test
                // configurations will use IP addresses and the internal
                // 9020/9021 ports.

                config.ProxyHost = endpoint.Host;
                config.ProxyPort = endpoint.Port;
                if (endpoint.Scheme == "https")
                {
                    throw new ArgumentException("ViPR S3 via HTTPS is not supported through .NET with the AWS SDK: " + endpoint);
                }
                else
                {
                    config.CommunicationProtocol = Protocol.HTTP;
                }
            }
            else
            {
                config.ServiceURL = endpoint.AbsoluteUri;
            }

            AWSCredentials creds = new BasicAWSCredentials(
                ConfigurationManager.AppSettings["AWSAccessKey"],
                ConfigurationManager.AppSettings["AWSSecretKey"]);
            var client = new AmazonS3Client(creds, config);
            return client;
        }

    }
}
