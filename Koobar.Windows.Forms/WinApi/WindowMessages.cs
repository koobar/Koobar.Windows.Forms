namespace Koobar.Windows.Forms.WinApi
{
    internal static class WindowMessages
    {
        public const int WM_PAINT = 0x000F;
        public const int WM_ERASEBKGND = 0x0014;
        public const int WM_PRINT = 0x0317;
        public const int WM_CONTEXTMENU = 0x7b;

        // WM_PRINTメッセージ
        public const int PRF_CHECKVISIBLE = 0x00000001;
        public const int PRF_NONCLIENT = 0x00000002;
        public const int PRF_CLIENT = 0x00000004;
        public const int PRF_ERASEBKGND = 0x00000008;
        public const int PRF_CHILDREN = 0x00000010;
    }
}
