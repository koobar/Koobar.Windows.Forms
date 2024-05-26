using System.Drawing;
using System.Windows.Forms;

namespace Koobar.Windows.Forms
{
    /// <summary>
    /// ClosableTabControl専用のタブページ
    /// </summary>
    public class TabPage
    {
        // コンストラクタ
        public TabPage(string text)
        {
            this.Text = text;
        }

        /// <summary>
        /// テキスト
        /// </summary>
        public string Text { set; get; }

        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackColor { set; get; } = SystemColors.Control;

        /// <summary>
        /// タブページに表示するコントロール
        /// </summary>
        public Control Control { set; get; }

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            if (this.Control != null)
            {
                this.Control.Dispose();
            }
        }
    }
}
