using System;

namespace Be.Windows.Forms
{
    internal static class Caret
    {
        public static bool Create(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight)
        {
            try
            {
                return NativeMethods.CreateCaret(hWnd, hBitmap, nWidth, nHeight);
            }
            catch (Exception ex)
            {
                // Let's pretend CreateCaret() is available on Linux
                if ((ex is DllNotFoundException) || (ex is EntryPointNotFoundException))
                    return true;

                throw;
            }
        }

        public static bool Show(IntPtr hWnd)
        {
            try
            {
                return NativeMethods.ShowCaret(hWnd);
            }
            catch (Exception ex)
            {
                // Let's pretend ShowCaret() is available on Linux
                if ((ex is DllNotFoundException) || (ex is EntryPointNotFoundException))
                    return true;

                throw;
            }
        }

        public static bool Destroy()
        {
            try
            {
                return NativeMethods.DestroyCaret();
            }
            catch (Exception ex)
            {
                // Let's pretend DestroyCaret() is available on Linux
                if ((ex is DllNotFoundException) || (ex is EntryPointNotFoundException))
                    return true;

                throw;
            }
        }

        public static bool SetPos(int X, int Y)
        {
            try
            {
                return NativeMethods.SetCaretPos(X, Y);
            }
            catch (Exception ex)
            {
                // Let's pretend SetCaretPos() is available on Linux
                if ((ex is DllNotFoundException) || (ex is EntryPointNotFoundException))
                    return true;

                throw;
            }
        }
    }
}
