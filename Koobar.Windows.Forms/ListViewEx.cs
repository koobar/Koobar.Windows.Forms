using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Koobar.Windows.Forms.WinApi.WindowMessages;

namespace Koobar.Windows.Forms
{
    public class ListViewEx : DoubleBufferedListView
    {
        // 非公開フィールド
        private readonly Dictionary<int, StringAlignment> subItemHorizontalTextAlignments;
        private readonly Dictionary<int, StringAlignment> subItemVerticalTextAlignments;
        private bool flagMouseEnter;
        
        // コンストラクタ
        public ListViewEx()
        {
            this.subItemHorizontalTextAlignments = new Dictionary<int, StringAlignment>();
            this.subItemVerticalTextAlignments = new Dictionary<int, StringAlignment>();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.ColumnHeaderFont = this.Font;
            this.ItemFont = this.Font;
            this.OwnerDraw = true;
        }

        #region プロパティ

        /// <summary>
        /// ヘッダのフォント
        /// </summary>
        public Font ColumnHeaderFont { set; get; }

        /// <summary>
        /// アイテムのフォント
        /// </summary>
        public Font ItemFont { set; get; }

        /// <summary>
        /// ヘッダのコンテキストメニュー
        /// </summary>
        public ContextMenuStrip ColumnHeaderContextMenuStrip { set; get; }

        /// <summary>
        /// アイテムのコンテキストメニュー
        /// </summary>
        public ContextMenuStrip ItemContextMenuStrip { set; get; }

        /// <summary>
        /// アイテムの境界線を描画するかどうか
        /// </summary>
        public bool DrawItemBorderLines { set; get; }

        /// <summary>
        /// ヘッダ部分の描画をオペレーティングシステムに任せるかどうか
        /// </summary>
        public bool DrawColumnHeaderBySystem { set; get; }

        /// <summary>
        /// 列ヘッダのテキストの水平方向の配置
        /// </summary>
        public StringAlignment ColumnHeaderTextHorizontalAlignment { set; get; } = StringAlignment.Center;

        /// <summary>
        /// 列ヘッダのテキストの垂直方向の配置
        /// </summary>
        public StringAlignment ColumnHeaderTextVerticalAlignment { set; get; } = StringAlignment.Center;
        
        /// <summary>
        /// コントロールをユーザー自身が描画するかどうか
        /// </summary>
        public new bool OwnerDraw
        {
            private set
            {
                base.OwnerDraw = value;
            }
            get
            {
                return base.OwnerDraw;
            }
        }

        #endregion

        /// <summary>
        /// 列の幅を、サイズ変更スタイルで示された幅に変更する。
        /// </summary>
        /// <param name="autoResizeStyle"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public new void AutoResizeColumns(ColumnHeaderAutoResizeStyle autoResizeStyle)
        {
            if (autoResizeStyle == ColumnHeaderAutoResizeStyle.None)
            {
                return;
            }
            else if (autoResizeStyle == ColumnHeaderAutoResizeStyle.HeaderSize)
            {
                using (var g = CreateGraphics())
                {
                    foreach (var obj in this.Columns)
                    {
                        if (obj is ColumnHeader)
                        {
                            var header = (ColumnHeader)obj;
                            var size = g.MeasureString(header.Text, this.ColumnHeaderFont);

                            header.Width = (int)size.Width;
                        }
                    }
                }

                Invalidate();
            }
            else if (autoResizeStyle == ColumnHeaderAutoResizeStyle.ColumnContent)
            {
                using (var g = CreateGraphics())
                {
                    foreach (var obj in this.Items)
                    {
                        var item = (ListViewItem)obj;

                        for (int columnIndex = 0; columnIndex < this.Columns.Count; ++columnIndex)
                        {
                            float width = 0;
                            using (var sf = new StringFormat() { Alignment = GetSubItemHorizontalTextAlignment(columnIndex), LineAlignment = GetSubItemVerticalTextAlignment(columnIndex) })
                            {
                                if (columnIndex == 0)
                                {
                                    width = g.MeasureString(item.Text, this.ItemFont, 0, sf).Width;
                                }
                                else
                                {
                                    if (item.SubItems.Count > columnIndex)
                                    {
                                        width = g.MeasureString(item.SubItems[columnIndex].Text, this.ItemFont, 0, sf).Width;
                                    }
                                }

                                if (this.Columns[columnIndex].Width < width)
                                {
                                    this.Columns[columnIndex].Width = (int)Math.Round(width, MidpointRounding.AwayFromZero);
                                }
                            }
                        }
                    }
                }

                Invalidate();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 指定された列ヘッダのインデックスに対応する列に表示されるサブアイテムの水平方向の配置を設定する。
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="alignment"></param>
        public void SetSubItemAlignment(int columnIndex, StringAlignment alignment)
        {
            if (this.subItemHorizontalTextAlignments.ContainsKey(columnIndex))
            {
                this.subItemHorizontalTextAlignments[columnIndex] = alignment;
            }
            else
            {
                this.subItemHorizontalTextAlignments.Add(columnIndex, alignment);
            }

            Invalidate();
        }

        /// <summary>
        /// 指定された列ヘッダのインデックスに対応する列に表示されるサブアイテムの垂直方向の配置を設定する。
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="alignment"></param>
        public void SetSubItemLineAlignment(int columnIndex, StringAlignment alignment)
        {
            if (this.subItemVerticalTextAlignments.ContainsKey(columnIndex))
            {
                this.subItemVerticalTextAlignments[columnIndex] = alignment;
            }
            else
            {
                this.subItemVerticalTextAlignments.Add(columnIndex, alignment);
            }

            Invalidate();
        }

        /// <summary>
        /// 指定されたインデックスに対応するサブアイテムの水平方向の配置を取得する。
        /// </summary>
        /// <param name="subItemIndex"></param>
        /// <returns></returns>
        public StringAlignment GetSubItemHorizontalTextAlignment(int subItemIndex)
        {
            if (this.subItemHorizontalTextAlignments.ContainsKey(subItemIndex))
            {
                return this.subItemHorizontalTextAlignments[subItemIndex];
            }

            return StringAlignment.Near;
        }

        /// <summary>
        /// 指定されたインデックスに対応するサブアイテムの垂直方向の配置を取得する。
        /// </summary>
        /// <param name="subItemIndex"></param>
        /// <returns></returns>
        public StringAlignment GetSubItemVerticalTextAlignment(int subItemIndex)
        {
            if (this.subItemVerticalTextAlignments.ContainsKey(subItemIndex))
            {
                return this.subItemVerticalTextAlignments[subItemIndex];
            }

            return StringAlignment.Near;
        }

        /// <summary>
        /// 指定されたインデックスのアイテムとマウスカーソルが接触しているかどうかを取得する。
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        protected bool ItemContainsMouseCursor(int itemIndex)
        {
            return GetItemRect(itemIndex).Contains(PointToClient(Cursor.Position));
        }

        /// <summary>
        /// コントロールの領域にマウスが入った場合の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            this.flagMouseEnter = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        /// <summary>
        /// コントロールの領域からマウスが出た場合の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            this.flagMouseEnter = false;
            Invalidate();

            base.OnMouseLeave(e);
        }

        /// <summary>
        /// コントロールの領域内でマウスが移動した場合の処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.flagMouseEnter && (this.HotTracking || this.HoverSelection))
            {
                Invalidate();
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// ヘッダ部分の描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            if (this.DrawColumnHeaderBySystem)
            {
                e.DrawDefault = true;
            }
            else if (this.View == View.Details)
            {
                using (var fillBrush = new SolidBrush(Color.White))
                {
                    if (e.Bounds.Contains(PointToClient(Cursor.Position)))
                    {
                        fillBrush.Color = SystemColors.Menu;
                    }

                    using (var pen = new Pen(Color.FromArgb(229, 229, 229)))
                    {
                        // ヘッダを塗りつぶし、境界線を描画
                        e.Graphics.FillRectangle(fillBrush, e.Bounds);
                        e.Graphics.DrawLine(pen, e.Bounds.Right - 1, e.Bounds.Y, e.Bounds.Right - 1, e.Bounds.Bottom);
                        e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Bottom - 1, e.Bounds.Right - 1, e.Bounds.Bottom - 1);
                    }

                    // ヘッダのテキストを描画
                    using (var sf = new StringFormat() { Alignment = this.ColumnHeaderTextHorizontalAlignment, LineAlignment = this.ColumnHeaderTextVerticalAlignment })
                    {
                        e.Graphics.DrawString(this.Columns[e.ColumnIndex].Text, this.ColumnHeaderFont, Brushes.Black, e.Bounds, sf);
                    }
                }
            }

            base.OnDrawColumnHeader(e);
        }

        /// <summary>
        /// アイテムの描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            var captionColor = SystemBrushes.ControlText;

            if (e.Item.Selected || (this.HotTracking && ItemContainsMouseCursor(e.ItemIndex)))
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                captionColor = SystemBrushes.HighlightText;
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            }

            // 詳細表示以外の表示の場合、アイテムのテキストを描画
            if (this.View != View.Details)
            {
                using (var format = new StringFormat())
                {
                    // 描画位置を設定
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    // テキストを描画
                    e.Graphics.DrawString(e.Item.Text, this.ItemFont, captionColor, e.Bounds, format);
                }
            }

            // アイテムの境界線を描画
            if (this.DrawItemBorderLines)
            {
                using (var pen = new Pen(Color.FromArgb(229, 229, 229)))
                {
                    // サブアイテムの境界線を描画
                    e.Graphics.DrawLine(pen, e.Bounds.Right - 1, e.Bounds.Y, e.Bounds.Right - 1, e.Bounds.Bottom);
                }
            }

            base.OnDrawItem(e);
        }

        /// <summary>
        /// サブアイテムの描画処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            string caption = e.SubItem.Text;

            using (var pen = new Pen(Color.FromArgb(229, 229, 229)))
            {
                var captionColor = SystemBrushes.ControlText;

                // 選択されているアイテムか？
                if (e.Item.Selected || (this.HotTracking && ItemContainsMouseCursor(e.ItemIndex)))
                {
                    captionColor = SystemBrushes.HighlightText;
                }

                using (var format = new StringFormat() { Alignment = GetSubItemHorizontalTextAlignment(e.ColumnIndex), LineAlignment = GetSubItemVerticalTextAlignment(e.ColumnIndex) })
                {
                    e.Graphics.DrawString(caption, this.ItemFont, captionColor, e.Bounds, format);
                }

                if (this.DrawItemBorderLines)
                {
                    // サブアイテムの境界線を描画
                    e.Graphics.DrawLine(pen, e.Bounds.Right - 1, e.Bounds.Y, e.Bounds.Right - 1, e.Bounds.Bottom);
                }
            }

            base.OnDrawSubItem(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
        }

        /// <summary>
        /// ウィンドウプロシージャ
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_CONTEXTMENU)
            {
                if (m.WParam != this.Handle)
                {
                    if (this.ColumnHeaderContextMenuStrip != null)
                    {
                        this.ColumnHeaderContextMenuStrip.Show(MousePosition);
                    }
                }
                else
                {
                    if (this.ItemContextMenuStrip != null)
                    {
                        this.ItemContextMenuStrip.Show(MousePosition);
                    }
                    else if (this.ContextMenuStrip != null)
                    {
                        this.ContextMenuStrip.Show(MousePosition);
                    }
                }
            }

            base.WndProc(ref m);
        }
    }
}
