using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Koobar.Windows.Forms.ControlElements
{
    internal partial class ClosableTabControlHeaderPanel : UserControl
    {
        // 非公開フィールド
        private readonly List<TabItem> tabItems;
        private readonly List<TabPage> tabPages;
        private TabPage selectedTabPage;

        // イベント
        public event EventHandler<TabPageEventArgs> TabClosed;
        public event EventHandler AnyTabItemClick;

        // コンストラクタ
        public ClosableTabControlHeaderPanel()
        {
            InitializeComponent();
            
            this.tabItems = new List<TabItem>();
            this.tabPages = new List<TabPage>();
            this.AutoSize = false;
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        #region プロパティ

        /// <summary>
        /// 選択されたタブのコンテンツ
        /// </summary>
        public TabPage SelectedTabPage
        {
            set
            {
                this.selectedTabPage = value;
                InvalidateAllTabItems();
            }
            get
            {
                return this.selectedTabPage;
            }
        }

        /// <summary>
        /// タブページ数
        /// </summary>
        public int TabCount
        {
            get
            {
                return this.tabPages.Count;
            }
        }

        #endregion

        #region 公開メソッド

        /// <summary>
        /// タブページを追加する。
        /// </summary>
        /// <param name="tabPage"></param>
        public void AddTabPage(TabPage tabPage)
        {
            var tabItem = new TabItem(tabPage);
            tabItem.Text = tabPage.Text;
            tabItem.Dock = DockStyle.Left;
            tabItem.TabItemClick += OnTabItemClick;
            tabItem.CloseButtonClick += OnTabClosed;

            this.tabPages.Insert(0, tabPage);
            this.tabItems.Insert(0, tabItem);

            this.Controls.Clear();
            foreach (var item in this.tabItems)
            {
                this.Controls.Add(item);
            }
        }

        /// <summary>
        /// 指定されたタブページを削除する。
        /// </summary>
        /// <param name="tabPage"></param>
        public void RemoveTabPage(TabPage tabPage)
        {
            RemoveTabPageAt(this.tabPages.IndexOf(tabPage));
        }

        /// <summary>
        /// 指定されたインデックスのタブページを削除する。
        /// </summary>
        /// <param name="index"></param>
        public void RemoveTabPageAt(int index)
        {
            if (index == -1)
            {
                return;
            }

            var tabItem = this.tabItems[index];
            var tabPage = this.tabPages[index];

            if (tabPage != null)
            {
                if (tabPage.Control != null)
                {
                    tabPage.Control.Visible = false;
                }

                tabPage.Dispose();
            }

            this.Controls.Remove(tabItem);
            this.tabItems.RemoveAt(index);
            this.tabPages.RemoveAt(index);
        }
        
        /// <summary>
        /// 指定されたインデックスのタブページを取得する。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TabPage GetTabPage(int index)
        {
            return this.tabPages[index];
        }

        #endregion

        /// <summary>
        /// 指定されたインデックスのタブのつまみの矩形を取得する。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected Rectangle GetTabItemRect(int index)
        {
            return this.tabItems[index].ClientRectangle;
        }

        /// <summary>
        /// 指定されたインデックスのタブの閉じるボタンの矩形を取得する。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected Rectangle GetCloseButtonRect(int index)
        {
            return this.tabItems[index].GetCloseButtonRect();
        }

        /// <summary>
        /// すべてのタブアイテムを再描画する。
        /// </summary>
        private void InvalidateAllTabItems()
        {
            for (int i = 0; i < this.tabItems.Count; ++i)
            {
                this.tabItems[i].DrawAsSelected = this.tabItems[i].TabPage == this.selectedTabPage;
                this.tabItems[i].Invalidate();
            }
        }

        /// <summary>
        /// タブが選択された場合の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabItemClick(object sender, TabPageEventArgs e)
        {
            this.SelectedTabPage = e.TabPage;
            this.AnyTabItemClick?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// タブの閉じるボタンがクリックされた場合の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabClosed(object sender, TabPageEventArgs e)
        {
            RemoveTabPage(e.TabPage);

            this.TabClosed?.Invoke(sender, e);
        }
    }
}
