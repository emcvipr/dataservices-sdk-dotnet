using Amazon.S3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AWSSDK.Test
{
    [TestClass]
    public class NamespaceTests
    {
        [TestMethod]
        public void TestNamespaceInHeader()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            var ns = appConfig["AWSNamespace"];
            Assert.IsNotNull(ns);

            var config = new AmazonS3Config()
                .WithNamespace(ns);

            using (var client = ClientTests.CreateClient(config: config))
            {
                var buckets = client.ListBuckets();
                foreach (var bucket in buckets.Buckets)
                {
                    Debug.WriteLine(string.Format("Bucket: {0}", bucket.BucketName));
                }
            }
        }

        [TestMethod]
        public void TestInvalidNamespace()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            string ns = Guid.NewGuid().ToString("N");

            var config = new AmazonS3Config()
                .WithNamespace(ns);

            using (var client = ClientTests.CreateClient(config: config))
            {
                try
                {
                    using (var response = client.ListBuckets())
                    {
                        Assert.Fail("expected failure");
                    }
                }
                catch (AmazonS3Exception ex)
                {
                    Assert.AreEqual("InvalidNamespace", ex.ErrorCode);
                }
            }
        }
    }
}
