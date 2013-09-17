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
