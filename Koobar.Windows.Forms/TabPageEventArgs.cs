using System;

namespace Koobar.Windows.Forms
{
    public class TabPageEventArgs : EventArgs
    {
        // 非公開フィールド
        private readonly TabPage tabPage;

        // コンストラクタ
        public TabPageEventArgs(TabPage tabPage)
        {
            this.tabPage = tabPage;
        }

        public TabPage TabPage
        {
            get
            {
                return this.tabPage;
            }
        }
    }
}
