using System;
using System.Drawing;
using System.Windows.Forms;
using static Koobar.Windows.Forms.WinApi.User32;
using static Koobar.Windows.Forms.WinApi.WindowMessages;

namespace Koobar.Windows.Forms
{
    public class DoubleBufferedListView : ListView
    {
        /// <summary>
        /// ウィンドウプロシージャ
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PAINT:
                    PaintControl(ref m);
                    break;
                case WM_ERASEBKGND:
                    // 無視
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// コントロールを描画する。
        /// </summary>
        /// <param name="msg"></param>
        /// <exception cref="Exception"></exception>
        private void PaintControl(ref Message msg)
        {
            if (msg.WParam == IntPtr.Zero)
            {
                var paintStruct = new PAINTSTRUCT();
                var deviceContext = BeginPaint(msg.HWnd, ref paintStruct);

                try
                {
                    using (var bufferedGraphics = BufferedGraphicsManager.Current.Allocate(deviceContext, this.ClientRectangle))
                    {
                        var clip = Rectangle.FromLTRB(paintStruct.rcPaint.Left, paintStruct.rcPaint.Top, paintStruct.rcPaint.Right, paintStruct.rcPaint.Bottom);

                        // 描画処理
                        bufferedGraphics.Graphics.SetClip(clip);
                        Draw(bufferedGraphics.Graphics, clip);
                        bufferedGraphics.Render();
                    }
                }
                catch
                {
                    throw new Exception("DoubleBufferedListViewの描画中に例外が発生しました。");
                }
                finally
                {
                    EndPaint(msg.HWnd, ref paintStruct);
                }
            }
            else
            {
                using (var g = Graphics.FromHdc(msg.WParam))
                {
                    Draw(g, this.ClientRectangle);
                }
            }
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clip"></param>
        private void Draw(Graphics graphics, Rectangle clip)
        {
            // 背景を塗りつぶす
            using (var brush = new SolidBrush(this.BackColor))
            {
                graphics.FillRectangle(brush, clip);

                IntPtr hdc = graphics.GetHdc();

                if (hdc == IntPtr.Zero)
                {
                    return;
                }

                try
                {
                    var msg = Message.Create(this.Handle, WM_PAINT, hdc, (IntPtr)(PRF_CHILDREN | PRF_CLIENT | PRF_ERASEBKGND));

                    // メッセージをウィンドウプロシージャに送信
                    DefWndProc(ref msg);
                }
                finally
                {
                    graphics.ReleaseHdc();
                }
            }
        }
    }
}
