using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon
{
    internal class ViPRConstants
    {
        
        // Header names
        internal const string EMC_PREFIX = "x-emc-";

        internal const string NAMESPACE_HEADER = "x-emc-namespace";
        internal const string APPEND_OFFSET_HEADER = "x-emc-append-offset";
        internal const string FILE_ACCESS_MODE_HEADER = "x-emc-file-access-mode";
        internal const string FILE_ACCESS_DURATION_HEADER = "x-emc-file-access-duration";
        internal const string FILE_ACCESS_HOST_LIST_HEADER = "x-emc-file-access-host-list";
        internal const string FILE_ACCESS_UID_HEADER = "x-emc-file-access-uid";
        internal const string FILE_ACCESS_TOKEN_HEADER = "x-emc-file-access-token";
        internal const string FILE_ACCESS_START_TOKEN_HEADER = "x-emc-file-access-start-token";
        internal const string FILE_ACCESS_END_TOKEN_HEADER = "x-emc-file-access-end-token";
    
        // Parameter names
        internal const string ACCESS_MODE_PARAMETER = "accessmode";
        internal const string FILE_ACCESS_PARAMETER = "fileaccess";
        internal const string MARKER_PARAMETER = "marker";
        internal const string MAX_KEYS_PARAMETER = "max-keys";
    }


    public enum FileAccessMode
    {
        Disabled,
        ReadOnly,
        ReadWrite,
        SwitchingToDisabled,
        SwitchingToReadOnly,
        SwitchingToReadWrite
    }

    public enum FileAccessProtocol
    {
        NFS,
        CIFS
    }
}
