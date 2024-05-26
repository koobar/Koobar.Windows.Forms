using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Koobar.Windows.Forms
{
    /// <summary>
    /// Windows 10ネイティブの見た目に近いデザインのMenuStrip
    /// </summary>
    public class NativeMenuStrip : MenuStrip
    {
        // 非公開定数
        private const int HEIGHT = 20;

        #region プロパティ

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new ToolStripRenderMode RenderMode
        {
            private set
            {
                base.RenderMode = value;
            }
            get
            {
                return base.RenderMode;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Padding Padding
        {
            private set
            {
                base.Padding = value;
            }
            get
            {
                return base.Padding;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AutoSize
        {
            private set
            {
                base.AutoSize = value;
            }
            get
            {
                return base.AutoSize;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new int Height
        {
            private set
            {
                base.Height = value;
            }
            get
            {
                return base.Height;
            }
        }

        public new Size Size
        {
            set
            {
                base.Size = value;
            }
            get
            {
                return base.Size;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor
        {
            private set
            {
                base.BackColor = value;
            }
            get
            {
                return base.BackColor;
            }
        }

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (this.DesignMode)
            {
                return;
            }

            base.Padding = new Padding(0, 0, 0, 0);
            base.AutoSize = false;
            base.Height = HEIGHT;
            base.BackColor = Color.White;
            base.RenderMode = ToolStripRenderMode.System;
            base.Size = new Size(this.Parent.Width, HEIGHT);
        }
    }
}
