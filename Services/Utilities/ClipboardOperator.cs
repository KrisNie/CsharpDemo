using System;
using System.Runtime.InteropServices;

namespace Services.Utilities
{
    public static class ClipboardOperator
    {
        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern bool SetClipboardData(uint format, IntPtr data);
        
        /// <summary>
        /// Copy data to clipboard
        /// </summary>
        /// <param name="data"></param>
        public static void CopyToClipboard(string data)
        {
            OpenClipboard(IntPtr.Zero);
            var ptr = Marshal.StringToHGlobalUni(data);
            SetClipboardData(13, ptr);
            CloseClipboard();
            Marshal.FreeHGlobal(ptr);
        }
    }
}