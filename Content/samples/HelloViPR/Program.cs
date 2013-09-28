using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using Amazon.Runtime;
using Amazon.S3.Util;

namespace HelloViPR
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = CreateClient();

            // create a bucket to contain an object
            string bucketName = Guid.NewGuid().ToString("N");
            var putBucketRequest = new PutBucketRequest()
            {
                BucketName = bucketName
            };
            client.PutBucket(putBucketRequest).Dispose();
            Console.WriteLine("Created a bucket named {0}.", putBucketRequest.BucketName);

            // create an object in the bucket
            var putObjectRequest = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = "MyObject.txt",
                ContentBody = "Hello World!"
            };
            client.PutObject(putObjectRequest).Dispose();
            Console.WriteLine("Created an object named {0}.", putObjectRequest.Key);
            
            // update the object in the bucket
            var updateObjectRequest = new UpdateObjectRequest()
            {
                BucketName = bucketName,
                Key = "MyObject.txt",
                ContentBody = "Again",
            };
            updateObjectRequest.UpdateRange = new Amazon.S3.Model.Tuple<long, long?>(6, null);
            client.UpdateObject(updateObjectRequest);
            Console.WriteLine("Updated the object's content.");

            // get the object content
            var getObjectRequest = new GetObjectRequest()
            {
                BucketName = bucketName,
                Key = "MyObject.txt"
            };
            using (var getObjectResponse = client.GetObject(getObjectRequest))
            {
                string contentBody = getObjectResponse.GetResponseContentBody();
                Console.WriteLine("The object content is: {0}.", contentBody);
            }
        
            // delete the bucket
            client.DeleteBucketRecursive(bucketName);
            Console.WriteLine("Deleted the bucket.");
        }

        private static AmazonS3Client CreateClient(AmazonS3Config config = null)
        {
            if (config == null) config = new AmazonS3Config();

            // load the ViPR S3 endpoint information and credentials from app.config
            Uri endpoint = new Uri(ConfigurationManager.AppSettings["AWSServiceURI"]);
            AWSCredentials creds = new BasicAWSCredentials(
                ConfigurationManager.AppSettings["AWSAccessKey"],
                ConfigurationManager.AppSettings["AWSSecretKey"]);

            // establish the configuration, for either a host-based endpoint or an IP-based endpoint
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
                config.CommunicationProtocol =
                        string.Equals("http", endpoint.Scheme, StringComparison.InvariantCultureIgnoreCase) ?
                        Protocol.HTTP : Protocol.HTTPS;
                config.ServiceURL = endpoint.GetComponents(UriComponents.HostAndPort, UriFormat.UriEscaped);
            }

            var client = new AmazonS3Client(creds, config);
            return client;
        }
    }

}
