using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text;

using Amazon.S3;
using Amazon.Runtime;
using Amazon.S3.Model;

namespace AWSSDK.Test
{
    /// <summary>
    /// Tests append functionality.
    /// </summary>
    /// <remarks>
    /// The test is initialized as follows:
    /// [class init]: initialize bucket
    /// [test init]: initialize object
    /// </remarks>
    [TestClass]
    public class AppendObjectTests
    {
        private const string ObjectContent_Before = "2+2=";
        private const string ObjectContent_Delta = "4";
        private const string ObjectContent_After = ObjectContent_Before + ObjectContent_Delta;

        private static AmazonS3 client;
        private static S3Bucket bucket;
        private string objKey;

        #region Initialization
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            try
            {
                client = ClientTests.CreateClient();

                var buckets = client.ListBuckets();
                bucket = buckets.Buckets.First();
            }
            catch (Exception e)
            {
                Assert.Inconclusive("prerequisite: unable to create client or bucket.  Error: {0}", e.Message);
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            client.Dispose();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            try
            {
                this.objKey = Guid.NewGuid().ToString("N");
                var putRequest = new PutObjectRequest()
                    .WithBucketName(bucket.BucketName)
                    .WithKey(this.objKey)
                    .WithContentBody(ObjectContent_Before);
                putRequest.GenerateMD5Digest = true;

                using (var putResponse = client.PutObject(putRequest)) { }
            }
            catch (Exception e)
            {
                Assert.Inconclusive("prerequisite: unable to create object.  Error: {0}", e.Message);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var deleteRequest = new DeleteObjectRequest()
                .WithBucketName(bucket.BucketName)
                .WithKey(this.objKey);

            using (var deleteResponse = client.DeleteObject(deleteRequest)) { }
        }
        #endregion

        [TestMethod]
        public void TestAppendObject()
        {
            var appendRequest = (AppendObjectRequest) new AppendObjectRequest()
                .WithBucketName(bucket.BucketName)
                .WithKey(this.objKey)
                .WithContentBody(ObjectContent_Delta);

            using (var appendResponse = client.AppendObject(appendRequest))
            {
                Assert.AreEqual(Encoding.UTF8.GetByteCount(ObjectContent_Before), appendResponse.AppendOffset);
            }

            // check content
            var getRequest = new GetObjectRequest()
                .WithBucketName(bucket.BucketName)
                .WithKey(this.objKey);
            using(var getResponse = client.GetObject(getRequest))
            {
                string contentBody = getResponse.GetResponseContentBody();
                Assert.AreEqual(ObjectContent_After, contentBody);
            }
        }
    }
}
