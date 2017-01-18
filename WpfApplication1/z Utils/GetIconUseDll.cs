﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;

// Where did I get this sample?
// For almost same code see http://www.leghumped.com/blog/2008/03/23/retrieving-shell-icons-in-c/

/// <summary>
/// Retrievs shell info associated with a file or filetype
/// </summary>
/// <summary>
/// Get a 32x32 or 16x16 System.Drawing.Icon depending on which function you call
/// either GetSmallIcon(string fileName) or GetLargeIcon(string fileName)
/// </summary>
/// 
namespace Utils
{
    public class ShellIcon
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };
        class Win32
        {
            public const uint SHGFI_ICON = 0x100;
            public const uint SHGFI_LARGEICON = 0x0; // Large icon
            public const uint SHGFI_SMALLICON = 0x1; // Small icon
            public const uint USEFILEATTRIBUTES = 0x000000010; // when the full path isn't available
            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
            [DllImport("User32.dll")]
            public static extern int DestroyIcon(IntPtr hIcon);

            //extra
            [DllImport("Shell32.dll")]
            public extern static int ExtractIconEx(string libName, int iconIndex, IntPtr[] largeIcon, IntPtr[] smallIcon, uint nIcons);
        }

        public ShellIcon() { }
        public static Icon GetSmallIcon(string fileName)
        {
            IntPtr hImgSmall; //the handle to the system image list
            SHFILEINFO shinfo = new SHFILEINFO();
            hImgSmall = Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_SMALLICON);
            //The icon is returned in the hIcon member of the shinfo struct
            Icon icon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();
            Win32.DestroyIcon(shinfo.hIcon);
            return icon;
        }
        public static Icon GetLargeIcon(string fileName)
        {
            IntPtr hImgLarge; //the handle to the system image list
            SHFILEINFO shinfo = new SHFILEINFO();
            hImgLarge = Win32.SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), Win32.SHGFI_ICON | Win32.SHGFI_LARGEICON);
            try
            {
                Icon icon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();
                Win32.DestroyIcon(shinfo.hIcon);
                return icon;
            }
            catch
            { 
                // to do, test registry??
                return null;
            }
        }
    }
}
