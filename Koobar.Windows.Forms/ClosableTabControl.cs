using System;
using System.Windows.Forms;

namespace Koobar.Windows.Forms
{
    /// <summary>
    /// 閉じるボタン付きのタブコントロール
    /// </summary>
    public partial class ClosableTabControl : UserControl
    {
        // 非公開フィールド
        private TabPage selectedTab;

        // コンストラクタ
        public ClosableTabControl()
        {
            InitializeComponent();

            this.TabHeaderSpace.AnyTabItemClick += OnAnyTabItemClick;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        #region プロパティ

        /// <summary>
        /// 選択されているタブ
        /// </summary>
        public TabPage SelectedTab
        {
            set
            {
                OnSelectedTabPageChanging(value);
            }
            get
            {
                return this.selectedTab;
            }
        }

        /// <summary>
        /// 選択されているタブのインデックス
        /// </summary>
        public int SelectedIndex
        {
            set
            {
                if (value >= 0 && this.TabHeaderSpace.TabCount - 1 >= value)
                {
                    this.SelectedTab = this.TabHeaderSpace.GetTabPage(value);
                }
            }
            get
            {
                for (int i = 0; i < this.TabHeaderSpace.TabCount; i++)
                {
                    if (this.TabHeaderSpace.GetTabPage(i) == this.selectedTab)
                    {
                        return i;
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// タブページ数
        /// </summary>
        public int TabCount
        {
            get
            {
                return this.TabHeaderSpace.TabCount;
            }
        }

        /// <summary>
        /// 境界線スタイル
        /// </summary>
        public new BorderStyle BorderStyle
        {
            set
            {
                this.TabPagePanel.BorderStyle = value;
            }
            get
            {
                return this.TabPagePanel.BorderStyle;
            }
        }

        #endregion

        /// <summary>
        /// タブページを追加する。
        /// </summary>
        /// <param name="tabPage"></param>
        public void AddTabPage(TabPage tabPage)
        {
            this.TabHeaderSpace.AddTabPage(tabPage);

            if (this.TabHeaderSpace.TabCount == 1)
            {
                this.SelectedIndex = 0;
            }

            this.TabHeaderSpace.Invalidate();
        }

        /// <summary>
        /// タブページを削除する。
        /// </summary>
        /// <param name="tabPage"></param>
        public void RemoveTabPage(TabPage tabPage)
        {
            this.TabHeaderSpace.RemoveTabPage(tabPage);

            // 再描画
            this.TabHeaderSpace.Invalidate();
            this.TabPagePanel.Invalidate();
        }

        /// <summary>
        /// 指定されたインデックスのタブページを削除する。
        /// </summary>
        /// <param name="index"></param>
        public void RemoveTabPageAt(int index)
        {
            this.TabHeaderSpace.RemoveTabPageAt(index);
        }

        /// <summary>
        /// 指定されたインデックスのタブページを追加する。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TabPage GetTabPage(int index)
        {
            return this.TabHeaderSpace.GetTabPage(index);
        }

        /// <summary>
        /// 指定されたタブページを表示する。
        /// </summary>
        /// <param name="tabPage"></param>
        protected void ShowTabPage(TabPage tabPage)
        {
            // 変更前に選択されているタブがあれば非表示化する。
            if (this.selectedTab != null && this.selectedTab.Control != null)
            {
                this.selectedTab.Control.Visible = false;
            }

            if (tabPage == null)
            {
                return;
            }

            // 選択されているタブを設定
            this.selectedTab = tabPage;
            this.TabPagePanel.BackColor = this.selectedTab.BackColor;

            // タブページに子コントロールが設定されていなければ何も表示しない。
            if (this.selectedTab.Control == null)
            {
                return;
            }

            // タブページに設定された子コントロールを表示する。
            this.selectedTab.Control.Parent = this.TabPagePanel;
            this.selectedTab.Control.Left = 2;
            this.selectedTab.Control.Top = 2;
            this.selectedTab.Control.Width = this.TabPagePanel.Width - 6;
            this.selectedTab.Control.Height = this.TabPagePanel.Height - 6;
            this.selectedTab.Control.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            this.selectedTab.Control.Visible = true;
        }

        /// <summary>
        /// いずれかのタブのつまみがクリックされた場合の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnAnyTabItemClick(object sender, EventArgs e)
        {
            ShowTabPage(this.TabHeaderSpace.SelectedTabPage);
        }

        /// <summary>
        /// 選択されたタブページが変更された場合の処理
        /// </summary>
        /// <param name="tabPage"></param>
        protected void OnSelectedTabPageChanging(TabPage tabPage)
        {
            ShowTabPage(tabPage);

            // タブの選択状態をヘッダ部分にも反映
            this.TabHeaderSpace.SelectedTabPage = tabPage;
        }
    }
}
