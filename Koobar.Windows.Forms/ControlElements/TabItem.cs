using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Koobar.Windows.Forms.ControlElements
{
    internal class TabItem : UserControl
    {
        // 非公開定数
        private const int CAPTION_LEFT_MARGIN = 3;
        private const int CLOSE_BUTTON_WIDTH = 15;
        private const int CLOSE_BUTTON_HEIGHT = 15;
        private const int CLOSE_BUTTON_LEFT_MARGIN = 5;
        private const int CLOSE_BUTTON_RIGHT_MARGIN = 3;
        private const int CLOSE_BUTTON_TOP_MARGIN = 3;

        // イベント
        public event EventHandler<TabPageEventArgs> CloseButtonClick;
        public event EventHandler<TabPageEventArgs> TabItemClick;

        // 非公開フィールド
        private readonly float drawingScale;
        private bool flagMouseEnter;

        // コンストラクタ
        public TabItem(TabPage tabPage)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.drawingScale = this.DeviceDpi / 96.0f;

            this.AutoSize = false;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.TabPage = tabPage;
        }

        #region プロパティ

        /// <summary>
        /// テキスト
        /// </summary>
        public new string Text
        {
            set
            {
                base.Text = value;

                UpdateWidth();
                Invalidate();
            }
            get
            {
                return base.Text;
            }
        }

        /// <summary>
        /// 対応するタブページ
        /// </summary>
        public TabPage TabPage { private set; get; }

        /// <summary>
        /// 選択された状態として描画するかどうか
        /// </summary>
        public bool DrawAsSelected { set; get; }

        #endregion

        /// <summary>
        /// タブアイテムの各要素の余白を計算する。
        /// </summary>
        /// <param name="captionLeftMargin"></param>
        /// <param name="closeButtonLeftMargin"></param>
        /// <param name="closeButtonRightMargin"></param>
        private void ComputeTabItemElementMargin(out int captionLeftMargin, out int closeButtonLeftMargin, out int closeButtonRightMargin)
        {
            captionLeftMargin = (int)(CAPTION_LEFT_MARGIN * this.drawingScale);
            closeButtonLeftMargin = (int)(CLOSE_BUTTON_LEFT_MARGIN * this.drawingScale);
            closeButtonRightMargin = (int)(CLOSE_BUTTON_RIGHT_MARGIN * this.drawingScale);
        }

        /// <summary>
        /// タブアイテムの各要素のサイズを計算する。
        /// </summary>
        /// <param name="closeButtonWidth"></param>
        /// <param name="closeButtonHeight"></param>
        private void ComputeTabItemElementSize(out int closeButtonWidth, out int closeButtonHeight)
        {
            closeButtonWidth = (int)(CLOSE_BUTTON_WIDTH * this.drawingScale);
            closeButtonHeight = (int)(CLOSE_BUTTON_HEIGHT * this.drawingScale);
        }

        /// <summary>
        /// コントロールの幅を更新する。
        /// </summary>
        private void UpdateWidth()
        {
            var g = CreateGraphics();

            ComputeTabItemElementSize(out int closeButtonWidth, out int closeButtonHeight);
            ComputeTabItemElementMargin(out int captionLeftMargin, out int closeButtonLeftMargin, out int closeButtonRightMargin);
            int width = (int)Math.Round(g.MeasureString(this.Text, this.Font).Width + closeButtonWidth + closeButtonLeftMargin + closeButtonRightMargin);

            this.ClientSize = new Size(width, this.ClientSize.Height);

            g.Dispose();
        }

        /// <summary>
        /// 閉じるボタンの矩形を取得する。
        /// </summary>
        /// <returns></returns>
        public Rectangle GetCloseButtonRect()
        {
            ComputeTabItemElementMargin(out _, out _, out int closeButtonRightMargin);
            ComputeTabItemElementSize(out int closeButtonWidth, out int closeButtonHeight);

            return new Rectangle(this.ClientRectangle.Right - closeButtonWidth - closeButtonRightMargin, this.Height / 2 - closeButtonHeight / 2, closeButtonWidth, closeButtonHeight);
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // タブのつまみの状態を取得する。
            var tabItemState = TabItemState.Normal;
            if (this.DrawAsSelected)
            {
                tabItemState = TabItemState.Selected;
            }
            else
            {
                if (this.flagMouseEnter)
                {
                    if (e.ClipRectangle.Contains(PointToClient(Cursor.Position)))
                    {
                        tabItemState = TabItemState.Hot;
                    }
                }
            }

            // タブを描画する。
            TabRenderer.DrawTabItem(e.Graphics, e.ClipRectangle, tabItemState);
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle, SystemColors.ControlText, TextFormatFlags.VerticalCenter);

            // 閉じるボタンを取得する。
            var closeButtonRect = GetCloseButtonRect();
            VisualStyleElement closeButton = VisualStyleElement.Window.CloseButton.Normal;
            if (this.flagMouseEnter)
            {
                if (closeButtonRect.Contains(PointToClient(Cursor.Position)))
                {
                    closeButton = VisualStyleElement.Window.CloseButton.Hot;
                }
            }

            // 閉じるボタンを描画する。
            var closeButtonRenderer = new VisualStyleRenderer(closeButton);
            closeButtonRenderer.DrawBackground(e.Graphics, closeButtonRect);
        }

        /// <summary>
        /// マウスがクリックされた場合の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (GetCloseButtonRect().Contains(PointToClient(Cursor.Position)))
                {
                    this.CloseButtonClick?.Invoke(this, new TabPageEventArgs(this.TabPage));
                }
                else
                {
                    this.TabItemClick?.Invoke(this, new TabPageEventArgs(this.TabPage));
                }
            }

            base.OnMouseClick(e);
        }

        /// <summary>
        /// マウスカーソルがコントロールの領域に入った場合の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            this.flagMouseEnter = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        /// <summary>
        /// マウスカーソルがコントロールの領域内で移動した場合処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            Invalidate();

            base.OnMouseMove(e);
        }

        /// <summary>
        /// マウスカーソルがコントロールの領域から出た場合の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            this.flagMouseEnter = false;
            Invalidate();

            base.OnMouseLeave(e);
        }
    }
}
