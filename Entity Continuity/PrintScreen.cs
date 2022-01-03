using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

public class ScreenCapture
{
    public static Image CaptureScreen()
    {
        IntPtr handle = User32.GetDesktopWindow();
        IntPtr hdcSrc = User32.GetWindowDC(handle);
        User32.RECT windowRect = new User32.RECT();

        User32.GetWindowRect(handle, ref windowRect);

        int width = 3840;
        int height = 2160;

        IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
        IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
        GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
        GDI32.SelectObject(hdcDest, GDI32.SelectObject(hdcDest, hBitmap));

        GDI32.DeleteDC(hdcDest);
        User32.ReleaseDC(handle, hdcSrc);
        Image img = Image.FromHbitmap(hBitmap);
        GDI32.DeleteObject(hBitmap);
        return img;
    }

    public static void CaptureScreenToFile(string filePath, string fileName)
    {
        CaptureScreen().Save(System.IO.Path.Combine(filePath, fileName), ImageFormat.Png);
    }

    /// Helper class containing Gdi32 API functions 
    private class GDI32
    {
        public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter 
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }


    /// Helper class containing User32 API functions 
    private class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern int SetProcessDPIAware();
    }
}