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

namespace Amazon.S3
{
    /// <summary>
    /// This interface contains the ViPR extensions above and beyond standard S3
    /// functionality. This includes object update, object append, and file access
    /// (e.g. NFS).
    /// </summary>
    public partial interface AmazonS3
    {
        UpdateObjectResponse UpdateObject(UpdateObjectRequest request);
        IAsyncResult BeginUpdateObject(UpdateObjectRequest request, AsyncCallback callback, object state);
        UpdateObjectResponse EndUpdateObject(IAsyncResult asyncResult);

        AppendObjectResponse AppendObject(AppendObjectRequest request);
        IAsyncResult BeginAppendObject(AppendObjectRequest request, AsyncCallback callback, object state);
        AppendObjectResponse EndAppendObject(IAsyncResult asyncResult);

        GetBucketFileAccessModeResponse GetBucketFileAccessMode(GetBucketFileAccessModeRequest request);
        IAsyncResult BeginGetBucketFileAccessMode(GetBucketFileAccessModeRequest request, AsyncCallback callback, object state);
        GetBucketFileAccessModeResponse EndGetBucketFileAccessMode(IAsyncResult asyncResult);

        SetBucketFileAccessModeResponse SetBucketFileAccessMode(SetBucketFileAccessModeRequest request);
        IAsyncResult BeginSetBucketFileAccessMode(SetBucketFileAccessModeRequest request, AsyncCallback callback, object state);
        SetBucketFileAccessModeResponse EndSetBucketFileAccessMode(IAsyncResult asyncResult);

        GetFileAccessResponse GetFileAccess(GetFileAccessRequest request);
        IAsyncResult BeginGetFileAccess(GetFileAccessRequest request, AsyncCallback callback, object state);
        GetFileAccessResponse EndGetFileAccess(IAsyncResult asyncResult);

    }
}
