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

using Amazon.S3.Model;
using Amazon.S3.Util;
using System.Net;
using Amazon.Runtime;

namespace Amazon.S3
{
    public partial class AmazonS3Client
    {
        #region Constructors
        /// <summary>
        /// Constructs AmazonS3Client with the credentials loaded from the application's
        /// default configuration, and if unsuccessful from the Instance Profile service on an EC2 instance.
        /// 
        /// Example App.config with credentials set. 
        /// <code>
        /// &lt;?xml version="1.0" encoding="utf-8" ?&gt;
        /// &lt;configuration&gt;
        ///     &lt;appSettings&gt;
        ///         &lt;add key="AWSAccessKey" value="********************"/&gt;
        ///         &lt;add key="AWSSecretKey" value="****************************************"/&gt;
        ///     &lt;/appSettings&gt;
        /// &lt;/configuration&gt;
        /// </code>
        ///
        /// </summary>
        /// <param name="serviceUri">The service endpoint to connect to.</param>
        public AmazonS3Client(Uri serviceUri)
            : this(FallbackCredentialsFactory.GetCredentials(), CreateConfig(serviceUri), true) { }

        /// <summary>
        /// Constructs AmazonS3Client with AWS Access Key ID, AWS Secret Key and an
        /// AmazonS3 Configuration object. If the config object's
        /// UseSecureStringForAwsSecretKey is false, the AWS Secret Key
        /// is stored as a clear-text string. Please use this option only
        /// if the application environment doesn't allow the use of SecureStrings.
        /// </summary>
        /// <param name="awsAccessKeyId">AWS Access Key ID</param>
        /// <param name="awsSecretAccessKey">AWS Secret Access Key</param>
        /// <param name="serviceUri">The service endpoint to connect to.</param>
        public AmazonS3Client(string awsAccessKeyId, string awsSecretAccessKey, Uri serviceUri)
            : this(CreateCredentials(awsAccessKeyId, awsSecretAccessKey), CreateConfig(serviceUri), true) { }
      
        /// <summary>
        /// Constructs an AmazonS3Client with AWSCredentials and an
        /// Amazon S3 Configuration object
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="serviceUri">The service endpoint to connect to.</param>
        public AmazonS3Client(AWSCredentials credentials, Uri serviceUri)
            : this(credentials, CreateConfig(serviceUri), false) { }

        private static AmazonS3Config CreateConfig(Uri serviceUri)
        {
            var config = new AmazonS3Config();
            config.CommunicationProtocol =
                string.Equals("http", serviceUri.Scheme, StringComparison.InvariantCultureIgnoreCase) ?
                Protocol.HTTP : Protocol.HTTPS;
            config.ServiceURL = serviceUri.GetComponents(UriComponents.HostAndPort, UriFormat.UriEscaped);
            return config;
        }
        #endregion

        #region UpdateObject
        public UpdateObjectResponse UpdateObject(UpdateObjectRequest request)
        {
            IAsyncResult asyncResult;
            asyncResult = invokeUpdateObject<UpdateObjectResponse>(request, null, null, true);
            return EndUpdateObject(asyncResult);
        }

        public IAsyncResult BeginUpdateObject(UpdateObjectRequest request, AsyncCallback callback, object state)
        {
            return invokeUpdateObject<UpdateObjectResponse>(request, callback, state, false);
        }

        public UpdateObjectResponse EndUpdateObject(IAsyncResult asyncResult)
        {
            var response = EndPutObject(asyncResult);
            return (UpdateObjectResponse) response;
        }

        IAsyncResult invokeUpdateObject<T>(UpdateObjectRequest request, AsyncCallback callback, object state, bool synchronized)
            where T : UpdateObjectResponse, new()
        {
            //WebHeaderCollection webHeaders = request.Headers;
            //webHeaders[HttpRequestHeader.Range] = "bytes=" + request.UpdateRange;
            var parameters = request.parameters;

            parameters[S3QueryParameter.Range] = String.Concat(
                    request.UpdateRange.First,
                    ":",
                    request.UpdateRange.Second
                    );

            return invokePutObject<T>(request, callback, state, synchronized);
        }

        #endregion

        #region AppendObject
        public AppendObjectResponse AppendObject(AppendObjectRequest request)
        {
            IAsyncResult asyncResult;
            asyncResult = invokeAppendObject<AppendObjectResponse>(request, null, null, true);
            return EndAppendObject(asyncResult);
        }

        public IAsyncResult BeginAppendObject(AppendObjectRequest request, AsyncCallback callback, object state)
        {
            return invokeAppendObject<AppendObjectResponse>(request, callback, state, false);
        }

        public AppendObjectResponse EndAppendObject(IAsyncResult asyncResult)
        {
            var response = (AppendObjectResponse) EndPutObject(asyncResult);
            response.AppendOffset = long.Parse(response.Headers[ViPRConstants.APPEND_OFFSET_HEADER]);
            return response;
        }

        IAsyncResult invokeAppendObject<T>(AppendObjectRequest request, AsyncCallback callback, object state, bool synchronized)
            where T : AppendObjectResponse, new()
        {
            return invokeUpdateObject<T>(request, callback, state, synchronized);
        }

        #endregion

        #region GetBucketFileAccess
        public GetBucketFileAccessModeResponse GetBucketFileAccessMode(GetBucketFileAccessModeRequest request)
        {
            IAsyncResult asyncResult;
            asyncResult = invokeGetBucketFileAccessMode(request, null, null, true);
            return EndGetBucketFileAccessMode(asyncResult);
        }

        public IAsyncResult BeginGetBucketFileAccessMode(GetBucketFileAccessModeRequest request, AsyncCallback callback, object state)
        {
            return invokeGetBucketFileAccessMode(request, callback, state, false);
        }

        public GetBucketFileAccessModeResponse EndGetBucketFileAccessMode(IAsyncResult asyncResult)
        {
            var response = endOperation<GetBucketFileAccessModeResponse>(asyncResult);

            var headers = response.Headers;
            if (headers[ViPRConstants.FILE_ACCESS_MODE_HEADER] != null)
                response.AccessMode = (FileAccessMode)Enum.Parse(typeof(FileAccessMode), headers[ViPRConstants.FILE_ACCESS_MODE_HEADER], true);
            if (headers[ViPRConstants.FILE_ACCESS_DURATION_HEADER] != null)
                response.Duration = long.Parse(headers[ViPRConstants.FILE_ACCESS_DURATION_HEADER]);
            if (headers[ViPRConstants.FILE_ACCESS_HOST_LIST_HEADER] != null)
                response.HostList = new List<string>(headers[ViPRConstants.FILE_ACCESS_HOST_LIST_HEADER].Split(','));
            if (headers[ViPRConstants.FILE_ACCESS_UID_HEADER] != null)
                response.Uid = headers[ViPRConstants.FILE_ACCESS_UID_HEADER];
            if (headers[ViPRConstants.FILE_ACCESS_START_TOKEN_HEADER] != null)
                response.StartToken = headers[ViPRConstants.FILE_ACCESS_START_TOKEN_HEADER];
            if (headers[ViPRConstants.FILE_ACCESS_END_TOKEN_HEADER] != null)
                response.EndToken = headers[ViPRConstants.FILE_ACCESS_END_TOKEN_HEADER];

            return response;
        }

        IAsyncResult invokeGetBucketFileAccessMode(GetBucketFileAccessModeRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(S3Constants.RequestParam, "The GetBucketFileAccessModeRequest is null!");
            }
            if (string.IsNullOrEmpty(request.BucketName))
            {
                throw new ArgumentNullException(S3Constants.RequestParam, "The bucket name parameter must be specified when querying access mode");
            }

            ConvertGetBucketFileAccessMode(request);
            S3AsyncResult asyncResult = new S3AsyncResult(request, state, callback, synchronized);
            invoke<GetBucketFileAccessModeResponse>(asyncResult);
            return asyncResult;
        }


        private void ConvertGetBucketFileAccessMode(GetBucketFileAccessModeRequest request)
        {
            var parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[S3QueryParameter.Verb] = S3Constants.GetVerb;
            parameters[S3QueryParameter.Action] = "GetBucketFileAccessMode";
            parameters[S3QueryParameter.Query] = parameters[S3QueryParameter.QueryToSign] = "?" + ViPRConstants.ACCESS_MODE_PARAMETER;

            request.RequestDestinationBucket = request.BucketName;
        }
        #endregion

        #region SetBucketFileAccess
        public SetBucketFileAccessModeResponse SetBucketFileAccessMode(SetBucketFileAccessModeRequest request)
        {
            IAsyncResult asyncResult;
            asyncResult = invokeSetBucketFileAccessMode(request, null, null, true);
            return EndSetBucketFileAccessMode(asyncResult);
        }

        public IAsyncResult BeginSetBucketFileAccessMode(SetBucketFileAccessModeRequest request, AsyncCallback callback, object state)
        {
            return invokeSetBucketFileAccessMode(request, callback, state, false);
        }

        public SetBucketFileAccessModeResponse EndSetBucketFileAccessMode(IAsyncResult asyncResult)
        {
            var response = endOperation<SetBucketFileAccessModeResponse>(asyncResult);

            var headers = response.Headers;
            if (headers[ViPRConstants.FILE_ACCESS_MODE_HEADER] != null)
                response.AccessMode = (FileAccessMode) Enum.Parse(typeof(FileAccessMode), headers[ViPRConstants.FILE_ACCESS_MODE_HEADER], true);
            if (headers[ViPRConstants.FILE_ACCESS_DURATION_HEADER] != null)
                response.Duration = long.Parse(headers[ViPRConstants.FILE_ACCESS_DURATION_HEADER]);
            if (headers[ViPRConstants.FILE_ACCESS_HOST_LIST_HEADER] != null)
                response.HostList = new List<string>(headers[ViPRConstants.FILE_ACCESS_HOST_LIST_HEADER].Split(','));
            if (headers[ViPRConstants.FILE_ACCESS_UID_HEADER] != null)
                response.Uid = headers[ViPRConstants.FILE_ACCESS_UID_HEADER];
            if (headers[ViPRConstants.FILE_ACCESS_START_TOKEN_HEADER] != null)
                response.StartToken = headers[ViPRConstants.FILE_ACCESS_START_TOKEN_HEADER];
            if (headers[ViPRConstants.FILE_ACCESS_END_TOKEN_HEADER] != null)
                response.EndToken = headers[ViPRConstants.FILE_ACCESS_END_TOKEN_HEADER];

            return response;
        }

        IAsyncResult invokeSetBucketFileAccessMode(SetBucketFileAccessModeRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(S3Constants.RequestParam, "The SetBucketFileAccessModeRequest is null!");
            }
            if (string.IsNullOrEmpty(request.BucketName))
            {
                throw new ArgumentNullException(S3Constants.RequestParam, "The bucket name parameter must be specified when changing access mode");
            }

            ConvertSetBucketFileAccessMode(request);
            S3AsyncResult asyncResult = new S3AsyncResult(request, state, callback, synchronized);
            invoke<SetBucketFileAccessModeResponse>(asyncResult);
            return asyncResult;
        }


        private void ConvertSetBucketFileAccessMode(SetBucketFileAccessModeRequest request)
        {
            var parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[S3QueryParameter.Verb] = S3Constants.PutVerb;
            parameters[S3QueryParameter.Action] = "SetBucketFileAccessMode";
            parameters[S3QueryParameter.Query] = parameters[S3QueryParameter.QueryToSign] = "?" + ViPRConstants.ACCESS_MODE_PARAMETER;

            parameters[S3QueryParameter.ContentType] = "application/xml";

            if (request.AccessMode != null)
            {
                webHeaders.Add(ViPRConstants.FILE_ACCESS_MODE_HEADER, request.AccessMode.ToString());
            }
            if (request.Duration > 0)
            { // TODO: is this an appropriate indicator?
                webHeaders.Add(ViPRConstants.FILE_ACCESS_DURATION_HEADER, request.Duration.ToString());
            }
            if (request.HostList != null && request.HostList.Count > 0)
            {
                webHeaders.Add(ViPRConstants.FILE_ACCESS_HOST_LIST_HEADER, string.Join(",", request.HostList.ToArray()));
            }
            if (request.Uid != null)
            {
                webHeaders.Add(ViPRConstants.FILE_ACCESS_UID_HEADER, request.Uid);
            }
            if (request.Token != null)
            {
                webHeaders.Add(ViPRConstants.FILE_ACCESS_TOKEN_HEADER, request.Token);
            }

            request.RequestDestinationBucket = request.BucketName;
        }
        #endregion

        #region GetFileAccess
        public GetFileAccessResponse GetFileAccess(GetFileAccessRequest request)
        {
            IAsyncResult asyncResult;
            asyncResult = invokeGetFileAccess(request, null, null, true);
            return EndGetFileAccess(asyncResult);
        }

        public IAsyncResult BeginGetFileAccess(GetFileAccessRequest request, AsyncCallback callback, object state)
        {
            return invokeGetFileAccess(request, callback, state, false);
        }

        public GetFileAccessResponse EndGetFileAccess(IAsyncResult asyncResult)
        {
            var response = endOperation<GetFileAccessResponse>(asyncResult);
            return response;
        }

        IAsyncResult invokeGetFileAccess(GetFileAccessRequest request, AsyncCallback callback, object state, bool synchronized)
        {
            if (request == null)
            {
                throw new ArgumentNullException(S3Constants.RequestParam, "The GetFileAccessRequest is null!");
            }
            if (string.IsNullOrEmpty(request.BucketName))
            {
                throw new ArgumentNullException(S3Constants.RequestParam, "The bucket name parameter must be specified when querying file access");
            }

            ConvertGetFileAccess(request);
            S3AsyncResult asyncResult = new S3AsyncResult(request, state, callback, synchronized);
            invoke<GetFileAccessResponse>(asyncResult);
            return asyncResult;
        }

        private void ConvertGetFileAccess(GetFileAccessRequest request)
        {
            var parameters = request.parameters;
            WebHeaderCollection webHeaders = request.Headers;

            parameters[S3QueryParameter.Verb] = S3Constants.GetVerb;
            parameters[S3QueryParameter.Action] = "GetFileAccess";
            parameters[S3QueryParameter.Query] = parameters[S3QueryParameter.QueryToSign] = "?" + ViPRConstants.FILE_ACCESS_PARAMETER;

            if (request.Marker != null)
            {
                webHeaders.Add(ViPRConstants.MARKER_PARAMETER, request.Marker);
            }
            if (request.MaxKeys > 0)
            {
                // TODO: is this an appropriate indicator?
                webHeaders.Add(ViPRConstants.MAX_KEYS_PARAMETER, request.MaxKeys.ToString());
            }

            request.RequestDestinationBucket = request.BucketName;
        }
        #endregion
    }
}
