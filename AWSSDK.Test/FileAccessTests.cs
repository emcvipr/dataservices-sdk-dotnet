using System;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Amazon.S3;
using Amazon.Runtime;
using Amazon.S3.Model;
using System.Diagnostics;
using Amazon;
using System.Collections.Generic;

namespace AWSSDK.Test
{
    /// <summary>
    /// Tests file access functionality.
    /// </summary>
    /// <remarks>
    /// The test is initialized as follows:
    /// [class init]: initialize client
    /// [test init]: initialize bucket /w object
    /// </remarks>
    [TestClass]
    public class FileAccessTests
    {
        private static AmazonS3 client;
        private string bucketName;

        #region Initialization
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            try
            {
                client = ClientTests.CreateClient();
            }
            catch (Exception e)
            {
                Assert.Inconclusive("prerequisite: unable to create client.  Error: {0}", e.Message);
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
                this.bucketName = Guid.NewGuid().ToString("N");
                var putRequest = new PutBucketRequest()
                {
                    BucketName = this.bucketName
                };
                using (var putResponse = client.PutBucket(putRequest)) { }

                var objRequest = new PutObjectRequest()
                {
                    BucketName = this.bucketName,
                    Key = "sample.txt",
                    ContentBody = "Hello!"
                };
                using (var objResponse = client.PutObject(objRequest)) { }
            }
            catch (Exception e)
            {
                Assert.Inconclusive("prerequisite: unable to create bucket.  Error: {0}", e.Message);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try 
            {
                var objRequest = new ListObjectsRequest()
                {
                    BucketName = this.bucketName
                };
                using (var objResponse = client.ListObjects(objRequest))
                {
                    var delRequest = new DeleteObjectsRequest()
                    {
                        BucketName = this.bucketName,
                        Quiet = true
                    };
                    delRequest.AddKeys(objResponse.S3Objects.Select(o => new KeyVersion(o.Key)).ToArray());

                    using (var delResponse = client.DeleteObjects(delRequest))
                    {
                        
                    }
                }

                var deleteRequest = new DeleteBucketRequest()
                {
                    BucketName = this.bucketName
                };
                using (var deleteResponse = client.DeleteBucket(deleteRequest)) { }
            } 
            catch (Exception ex) 
            {
                this.TestContext.WriteLine("Warning: Could not cleanup bucket: {0}.  {1}", this.bucketName, ex);
            }
        }
        #endregion

        private bool WaitForTransition(FileAccessMode targetMode, TimeSpan timeout)
        {
            DateTime timeoutTime = DateTime.Now + timeout;

            if (GetBucketFileAccessModeRequest.IsTransitionState(targetMode))
            {
                throw new ArgumentException("Invalid target mode: " + targetMode);
            }

            this.TestContext.WriteLine(
                "Waiting for file access mode transition. Bucket:{0}, Target: {1}", 
                this.bucketName, targetMode);

            while (true)
            {
                var request = new GetBucketFileAccessModeRequest()
                {
                    BucketName = this.bucketName
                };
                using (var response = client.GetBucketFileAccessMode(request))
                {
                    if (response.AccessMode == targetMode)
                    {
                        this.TestContext.WriteLine(
                            "Successful transition.  Target: {0}", targetMode);
                        return true;
                    }

                    if (DateTime.Now > timeoutTime)
                    {
                        this.TestContext.WriteLine(
                            "Timeout occurred waiting for transition.  Current: {0}, Target: {1}",
                            response.AccessMode, targetMode);
                        return false;
                    }
                }

                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }

        [TestMethod]
        public void TestGetBucketFileAccessMode()
        {
            var request = new GetBucketFileAccessModeRequest()
            {
                BucketName = this.bucketName
            };
            using (var response = client.GetBucketFileAccessMode(request))
            {
                Debug.WriteLine("AccessMode: " + response.AccessMode);
                Assert.AreEqual(FileAccessMode.Disabled, response.AccessMode);
            }
        }

        //[TestMethod]
        public void TestSetBucketFileAccessMode()
        {
            var setBucketRequest = new SetBucketFileAccessModeRequest()
            {
                BucketName = this.bucketName,
                AccessMode = FileAccessMode.ReadOnly,
                Duration = (long)TimeSpan.FromSeconds(300).TotalSeconds,
                HostList = new[] { "10.6.143.99", "10.6.143.100" }, // client IPs
                Uid = "501"
            };
            using (var setBucketResponse = client.SetBucketFileAccessMode(setBucketRequest))
            {
                Assert.IsTrue(
                    setBucketRequest.AccessMode == setBucketResponse.AccessMode ||
                    GetBucketFileAccessModeRequest.TransitionsToState((FileAccessMode)setBucketRequest.AccessMode, setBucketResponse.AccessMode),
                    "wrong access mode");

                Assert.IsTrue(setBucketRequest.Duration - setBucketResponse.Duration <= TimeSpan.FromSeconds(60).TotalSeconds, "wrong duration");
                Assert.IsTrue(setBucketRequest.HostList.SequenceEqual(setBucketResponse.HostList), "wrong host list");
                Assert.AreEqual(setBucketRequest.Uid, setBucketResponse.Uid, "wrong Uid");
            }

            if (!WaitForTransition(FileAccessMode.ReadOnly, TimeSpan.FromMinutes(5)))
            {
                Assert.Inconclusive("file access transition timed out");
            }

            var getAccessRequest = new GetFileAccessRequest()
            {
                BucketName = this.bucketName
            };
            using (var getAccessResponse = client.GetFileAccess(getAccessRequest))
            {
                Assert.IsTrue(getAccessResponse.MountPoints != null && getAccessResponse.MountPoints.Count >= 1, "missing mount points");
                Assert.IsTrue(getAccessResponse.Objects != null && getAccessResponse.Objects.Count == 1, "missing objects");
            }

            var setAccessMode2Request = new SetBucketFileAccessModeRequest()
            {
                BucketName = this.bucketName,
                AccessMode = FileAccessMode.Disabled
            };
            using (var getAccessMode2Response = client.SetBucketFileAccessMode(setAccessMode2Request))
            {
            }

            if (!WaitForTransition(FileAccessMode.Disabled, TimeSpan.FromMinutes(5)))
            {
                Assert.Inconclusive("file access transition timed out");
            }
        }
    }
}
