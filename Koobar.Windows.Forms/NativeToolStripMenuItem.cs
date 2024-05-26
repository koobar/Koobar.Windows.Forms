using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Koobar.Windows.Forms
{
    /// <summary>
    /// Windows 10ネイティブの見た目に近いデザインのToolStripMenuItem
    /// </summary>
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public class NativeToolStripMenuItem : ToolStripMenuItem
    {
        // 非公開フィールド
        private static readonly Color DefaultMenuItemBackColor = Color.White;
        private static readonly Color DefaultMenuItemBorderColor = Color.White;
        private static readonly Color OpenMenuItemBackColor = Color.FromArgb(204, 232, 255);
        private static readonly Color OpenMenuItemBorderColor = Color.FromArgb(153, 209, 255);
        private static readonly Color MouseEnterMenuItemBackColor = Color.FromArgb(229, 243, 255);
        private static readonly Color MouseEnterMenuItemBorderColor = Color.FromArgb(204, 232, 255);
        private static readonly Color DefaultDropDownMenuItemBackColor = Color.FromArgb(240, 240, 240);
        private static readonly Color DefaultDropDownMenuItemBorderColor = Color.FromArgb(240, 240, 240);
        private static readonly Color MouseEnterDropDownMenuItemBackColor = Color.FromArgb(144, 200, 246);
        private static readonly Color MouseEnterDropDownMenuItemBorderColor = Color.FromArgb(144, 200, 246);
        private static readonly Color CheckGlyphBackColorDefault = Color.FromArgb(144, 200, 246);
        private static readonly Color CheckGlyphBackColorSelected = Color.FromArgb(86, 176, 250);
        private readonly KeysConverter keysConverter = new KeysConverter();

        // フラグ
        private bool flagMouseEnter;
        private bool flagOpen;

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);

            SolidBrush bg = null;
            Pen bd = null;
            float x = 0, y = 0, right = 0, bottom = 0;

            if (this.IsOnDropDown)
            {
                x = e.ClipRectangle.X + 3;
                y = e.ClipRectangle.Y + 1;
                right = e.ClipRectangle.Right - 6;
                bottom = e.ClipRectangle.Bottom;

                if (this.flagMouseEnter)
                {
                    bg = new SolidBrush(MouseEnterDropDownMenuItemBackColor);
                    bd = new Pen(MouseEnterDropDownMenuItemBorderColor);
                }
                else
                {
                    bg = new SolidBrush(DefaultDropDownMenuItemBackColor);
                    bd = new Pen(DefaultDropDownMenuItemBorderColor);
                }

                // メニューの背景を塗りつぶす。
                e.Graphics.FillRectangle(bg, x, y, right, bottom);
                e.Graphics.DrawRectangle(bd, x, y, right, bottom);

                // テキストを描画する。
                e.Graphics.DrawString(this.Text.Replace("&", string.Empty), this.Font, SystemBrushes.MenuText, 32, 2.5f);

                // ショートカットキーを表示する。
                if (this.ShowShortcutKeys && this.ShortcutKeys != Keys.None)
                {
                    string s = this.keysConverter.ConvertToString(this.ShortcutKeys);
                    float sx = e.ClipRectangle.Width - 27 - e.Graphics.MeasureString(s, this.Font).Width;

                    e.Graphics.DrawString(s, this.Font, SystemBrushes.MenuText, sx, 2.5f);
                }

                // チェックされていればその状態を描画する。
                if (this.CheckState != CheckState.Unchecked)
                {
                    int posX = 3;
                    int posY = 1;
                    int sizeW = 22;
                    int sizeH = 22;

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                    // 選択されているメニューアイテムなら濃色で、そうでなければ通常色でチェックの背景部分を塗りつぶす。
                    if (this.Selected)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(CheckGlyphBackColorSelected), posX, posY, sizeW, sizeH);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(CheckGlyphBackColorDefault), posX, posY, sizeW, sizeH);
                    }

                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    switch (this.CheckState)
                    {
                        case CheckState.Checked:
                            var pen = new Pen(Color.Black, 1);
                            e.Graphics.DrawLine(pen, 9, 10, 12, 14);
                            e.Graphics.DrawLine(pen, 12, 14, 18, 6);
                            pen.Dispose();
                            break;
                        case CheckState.Indeterminate:
                            e.Graphics.FillEllipse(Brushes.Black, 10, 8, 6, 6);
                            break;
                    }
                }

                // サブメニューがあればその記号を描画する。
                if (this.DropDownItems.Count > 0)
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    var pen = new Pen(Color.Black, 1);
                    e.Graphics.DrawLine(pen, e.ClipRectangle.Right - 16, 8, e.ClipRectangle.Right - 13, 11);
                    e.Graphics.DrawLine(pen, e.ClipRectangle.Right - 16, 14, e.ClipRectangle.Right - 13, 11);
                    pen.Dispose();
                }
            }
            else
            {
                x = e.ClipRectangle.X;
                y = e.ClipRectangle.Y;
                right = e.ClipRectangle.Right - 1;
                bottom = e.ClipRectangle.Bottom - 3;

                if (this.flagOpen)
                {
                    bg = new SolidBrush(OpenMenuItemBackColor);
                    bd = new Pen(OpenMenuItemBorderColor);
                }
                else if (this.flagMouseEnter)
                {
                    bg = new SolidBrush(MouseEnterMenuItemBackColor);
                    bd = new Pen(MouseEnterMenuItemBorderColor);
                }
                else
                {
                    bg = new SolidBrush(DefaultMenuItemBackColor);
                    bd = new Pen(DefaultMenuItemBorderColor);
                }

                e.Graphics.FillRectangle(bg, x, y, right, bottom);
                e.Graphics.DrawRectangle(bd, x, y, right, bottom);

                // テキストを描画する。
                var displayString = this.Text.Replace("&", string.Empty);
                var sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(displayString, this.Font, SystemBrushes.MenuText, e.ClipRectangle.Width / 2, 0, sf);
            }

            // 後始末
            bg.Dispose();
            bd.Dispose();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.flagMouseEnter = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.flagMouseEnter = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        protected override void OnDropDownOpened(EventArgs e)
        {
            this.flagOpen = true;
            Invalidate();

            base.OnDropDownOpened(e);
        }

        protected override void OnDropDownHide(EventArgs e)
        {
            this.flagOpen = false;
            Invalidate();

            base.OnDropDownHide(e);
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            this.flagOpen = false;
            Invalidate();

            base.OnDropDownClosed(e);
        }
    }
}
