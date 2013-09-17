using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Amazon.S3.Model;

namespace AWSSDK.Test
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
    }
}
