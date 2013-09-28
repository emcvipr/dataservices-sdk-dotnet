using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloViPR
{
    internal static class AmazonS3Extensions
    {
        /// <summary>
        /// Gets the content body of the response.
        /// </summary>
        /// <remarks>
        /// The content body is assumed to be UTF8, consistent with the handling of <c>PutObjectRequest.ContentBody</c>.
        /// </remarks>
        /// <param name="response">The response to process.</param>
        /// <param name="encoding">The encoding of the body, or UTF8 if null.</param>
        /// <returns>The textual content of the response body.</returns>
        public static string GetResponseContentBody(this GetObjectResponse response, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            var s = new MemoryStream();
            BufferedStream bufferedStream = new BufferedStream(response.ResponseStream);
            byte[] buffer = new byte[ushort.MaxValue];
            int bytesRead = 0;
            while ((bytesRead = bufferedStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                s.Write(buffer, 0, bytesRead);
            }

            return encoding.GetString(s.ToArray());
        }

        /// <summary>
        /// Deletes all keys in a given bucket, then deletes the bucket itself.
        /// </summary>
        /// <param name="client">The client to use.</param>
        /// <param name="bucketName">The bucket to delete.</param>
        public static void DeleteBucketRecursive(this AmazonS3 client, string bucketName)
        {
            while(true)
            {
                // attempt to delete the bucket
                try
                {
                    var deleteRequest = new DeleteBucketRequest()
                    {
                        BucketName = bucketName
                    };
                    using (var deleteResponse = client.DeleteBucket(deleteRequest))
                    {
                        // deletion was successful
                        return;
                    }
                }
                catch (AmazonS3Exception ex)
                {
                    if (ex.ErrorCode != "BucketNotEmpty") throw ex;
                }

                var objRequest = new ListObjectsRequest()
                {
                    BucketName = bucketName
                };
                using (var objResponse = client.ListObjects(objRequest))
                {
                    var delRequest = new DeleteObjectsRequest()
                    {
                        BucketName = bucketName,
                        Quiet = true
                    };

                    // select the objects to delete (up to the supported limit of 1000)
                    var objToDelete = objResponse.S3Objects.Take(1000).Select(o => new KeyVersion(o.Key));
                    delRequest.AddKeys(objToDelete.ToArray());

                    using (var delResponse = client.DeleteObjects(delRequest))
                    {
                    }
                }
            }
        }
    }

}
