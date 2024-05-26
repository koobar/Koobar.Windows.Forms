using System;
using System.Windows.Forms;

namespace Koobar.Windows.Forms.ControlElements
{
    internal class TabPagePanel : Panel
    {
        public TabPagePanel()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            TabRenderer.DrawTabPage(e.Graphics, e.ClipRectangle);
            base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            Refresh();
            base.OnSizeChanged(e);
        }
    }
}
