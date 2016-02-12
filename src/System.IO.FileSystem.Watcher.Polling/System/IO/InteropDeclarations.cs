// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace System.IO.FileSystem
{
    class DllImports
    {
        internal static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        // dwAdditionalFlags:
        public const int FIND_FIRST_EX_CASE_SENSITIVE = 1;
        public const int FIND_FIRST_EX_LARGE_FETCH = 2;

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static unsafe extern IntPtr FindFirstFileW(string lpFileName, void* lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        internal static unsafe extern bool FindNextFileW(IntPtr hFindFile, void* lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool FindClose(IntPtr hFindFile);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static unsafe extern IntPtr FindFirstFileExW(string lpFileName, FINDEX_INFO_LEVELS fInfoLevelId, void* lpFindFileData, FINDEX_SEARCH_OPS fSearchOp, IntPtr lpSearchFilter, int dwAdditionalFlags);
    }

    enum FINDEX_INFO_LEVELS
    {
        FindExInfoStandard = 0,
        FindExInfoBasic = 1
    }

    enum FINDEX_SEARCH_OPS
    {
        FindExSearchNameMatch = 0,
        FindExSearchLimitToDirectories = 1,
        FindExSearchLimitToDevices = 2
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    unsafe struct WIN32_FIND_DATAW
    {
        public const int MAX_PATH = 260;
        public const int MAX_ALTERNATE = 14;

        public uint dwFileAttributes;
        public FILETIME ftCreationTime;
        public FILETIME ftLastAccessTime;
        public FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;

        public fixed char cFileName[MAX_PATH];
        public fixed char cAlternate[MAX_ALTERNATE];

        public ulong FileSize
        {
            get
            {
                ulong result = nFileSizeHigh << 32;
                result |= nFileSizeLow;
                return result;
            }
        }

        public ulong LastWrite
        {
            get
            {
                return ftLastWriteTime.Time;
            }
        }

        public bool IsDirectory
        {
            get
            {
                if ((dwFileAttributes & 16) != 0)
                {
                    return true;
                }
                return false;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct FILETIME
    {
        public uint TimeLow;
        public uint TimeHigh;

        public ulong Time
        {
            get
            {
                ulong result = TimeHigh << 32;
                result |= TimeLow;
                return result;
            }
        }
    }
}
