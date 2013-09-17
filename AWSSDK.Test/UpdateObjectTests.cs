using System;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Amazon.S3;
using Amazon.Runtime;
using Amazon.S3.Model;

namespace AWSSDK.Test
{
    /// <summary>
    /// Tests update functionality.
    /// </summary>
    /// <remarks>
    /// The test is initialized as follows:
    /// [class init]: initialize bucket
    /// [test init]: initialize object
    /// </remarks>
    [TestClass]
    public class UpdateObjectTests
    {
        private const string ObjectContent_Before = "Hello World!";
        private const string ObjectContent_Replace_Old = "World";
        private const string ObjectContent_Replace_New = "Again";
        private static readonly string ObjectContent_After = 
            ObjectContent_Before.Replace(ObjectContent_Replace_Old, ObjectContent_Replace_New);

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
        public void TestUpdateRange()
        {
            var before = ObjectContent_Before.Substring(0, ObjectContent_Before.IndexOf(ObjectContent_Replace_Old));

            var updateRequest = (UpdateObjectRequest)new UpdateObjectRequest()
                .WithUpdateRange(
                    Encoding.UTF8.GetByteCount(before),
                    Encoding.UTF8.GetByteCount(before) + Encoding.UTF8.GetByteCount(ObjectContent_Replace_New) - 1)
                .WithBucketName(bucket.BucketName)
                .WithKey(this.objKey)
                .WithContentBody(ObjectContent_Replace_New);

            using (var updateResponse = client.UpdateObject(updateRequest))
            {
            }

            // check content
            var getRequest = new GetObjectRequest()
                .WithBucketName(bucket.BucketName)
                .WithKey(this.objKey);
            using (var getResponse = client.GetObject(getRequest))
            {
                string contentBody = getResponse.GetResponseContentBody();
                Assert.AreEqual(ObjectContent_After, contentBody);
            }
        }

        [TestMethod]
        public void TestUpdateStartOffset()
        {
            var before = ObjectContent_Before.Substring(0, ObjectContent_Before.IndexOf(ObjectContent_Replace_Old));

            var updateRequest = (UpdateObjectRequest)new UpdateObjectRequest()
                .WithUpdateOffset(
                    Encoding.UTF8.GetByteCount(before))
                .WithBucketName(bucket.BucketName)
                .WithKey(this.objKey)
                .WithContentBody(ObjectContent_Replace_New);

            using (var updateResponse = client.UpdateObject(updateRequest))
            {
            }

            // check content
            var getRequest = new GetObjectRequest()
                .WithBucketName(bucket.BucketName)
                .WithKey(this.objKey);
            using (var getResponse = client.GetObject(getRequest))
            {
                string contentBody = getResponse.GetResponseContentBody();
                Assert.AreEqual(ObjectContent_After, contentBody);
            }
        }


        [TestMethod]
        public void TestInvalidUpdateRange()
        {
            var before = ObjectContent_Before.Substring(0, ObjectContent_Before.IndexOf(ObjectContent_Replace_Old));

            var updateRequest = (UpdateObjectRequest)new UpdateObjectRequest()
                .WithUpdateRange(
                    Encoding.UTF8.GetByteCount(ObjectContent_Before),
                    Encoding.UTF8.GetByteCount(before))
                .WithBucketName(bucket.BucketName)
                .WithKey(this.objKey)
                .WithContentBody(ObjectContent_Replace_New);

            try
            {
                using (var updateResponse = client.UpdateObject(updateRequest))
                {
                    Assert.Fail("expected UpdateObject to fail given an invalid range");
                }
            }
            catch (AmazonS3Exception ex)
            {
                Assert.AreEqual(HttpStatusCode.RequestedRangeNotSatisfiable, ex.StatusCode);
            }
        }

    }
}
