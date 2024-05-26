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
            Rectangle bgRect;

            if (this.IsOnDropDown)
            {
                bgRect = new Rectangle(e.ClipRectangle.X + 3, e.ClipRectangle.Y + 1, e.ClipRectangle.Right - 6, e.ClipRectangle.Bottom - 1);

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
                e.Graphics.FillRectangle(bg, bgRect);
                e.Graphics.DrawRectangle(bd, bgRect);

                // テキストを描画する。
                using (var sf = new StringFormat())
                {
                    sf.LineAlignment = StringAlignment.Center;
                    e.Graphics.DrawString(this.Text.Replace("&", string.Empty), this.Font, SystemBrushes.MenuText, new RectangleF(32, 0, bgRect.Width, bgRect.Height), sf);

                    // ショートカットキーを表示する。
                    if (this.ShowShortcutKeys && this.ShortcutKeys != Keys.None)
                    {
                        string s = this.keysConverter.ConvertToString(this.ShortcutKeys);
                        float sx = e.ClipRectangle.Width - 20 - e.Graphics.MeasureString(s, this.Font).Width;

                        e.Graphics.DrawString(s, this.Font, SystemBrushes.MenuText, new RectangleF(sx, 0, bgRect.Width - sx, bgRect.Height), sf);
                    }

                    // チェックされていればその状態を描画する。
                    if (this.CheckState != CheckState.Unchecked)
                    {
                        int sizeW = 22;
                        int sizeH = 22;

                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                        // 選択されているメニューアイテムなら濃色で、そうでなければ通常色でチェックの背景部分を塗りつぶす。
                        if (this.Selected)
                        {
                            e.Graphics.FillRectangle(new SolidBrush(CheckGlyphBackColorSelected), bgRect.X, bgRect.Y, sizeW, sizeH);
                        }
                        else
                        {
                            e.Graphics.FillRectangle(new SolidBrush(CheckGlyphBackColorDefault), bgRect.X, bgRect.Y, sizeW, sizeH);
                        }

                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        switch (this.CheckState)
                        {
                            case CheckState.Checked:
                                using (var pen = new Pen(Color.Black))
                                {
                                    e.Graphics.DrawLine(pen, 9, 10, 12, 14);
                                    e.Graphics.DrawLine(pen, 12, 14, 18, 8);
                                }
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

                        using (var pen = new Pen(Color.Black, 1))
                        {
                            e.Graphics.DrawLine(pen, e.ClipRectangle.Right - 16, 8, e.ClipRectangle.Right - 13, 11);
                            e.Graphics.DrawLine(pen, e.ClipRectangle.Right - 16, 14, e.ClipRectangle.Right - 13, 11);
                        }
                    }
                }
            }
            else
            {
                bgRect = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Right - 1, e.ClipRectangle.Bottom - 3);

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

                // メニューの背景を塗りつぶす。
                e.Graphics.FillRectangle(bg, bgRect);
                e.Graphics.DrawRectangle(bd, bgRect);

                // テキストを描画する。
                var displayString = this.Text.Replace("&", string.Empty);
                using (var sf = new StringFormat() { Alignment = StringAlignment.Center })
                {
                    e.Graphics.DrawString(displayString, this.Font, SystemBrushes.MenuText, e.ClipRectangle.Width / 2, 0, sf);
                }
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
