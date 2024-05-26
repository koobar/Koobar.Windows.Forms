using Koobar.Windows.Forms.ControlElements;

namespace Koobar.Windows.Forms
{
    partial class ClosableTabControl
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.TabPagePanel = new Koobar.Windows.Forms.ControlElements.TabPagePanel();
            this.TabHeaderSpace = new Koobar.Windows.Forms.ControlElements.ClosableTabControlHeaderPanel();
            this.SuspendLayout();
            // 
            // TabPagePanel
            // 
            this.TabPagePanel.BackColor = System.Drawing.SystemColors.Control;
            this.TabPagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabPagePanel.Location = new System.Drawing.Point(0, 20);
            this.TabPagePanel.Name = "TabPagePanel";
            this.TabPagePanel.Size = new System.Drawing.Size(800, 430);
            this.TabPagePanel.TabIndex = 1;
            // 
            // TabHeaderSpace
            // 
            this.TabHeaderSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.TabHeaderSpace.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            this.TabHeaderSpace.Location = new System.Drawing.Point(0, 0);
            this.TabHeaderSpace.Margin = new System.Windows.Forms.Padding(4);
            this.TabHeaderSpace.Name = "TabHeaderSpace";
            this.TabHeaderSpace.SelectedTabPage = null;
            this.TabHeaderSpace.Size = new System.Drawing.Size(800, 20);
            this.TabHeaderSpace.TabIndex = 0;
            // 
            // ClosableTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabPagePanel);
            this.Controls.Add(this.TabHeaderSpace);
            this.Name = "ClosableTabControl";
            this.Size = new System.Drawing.Size(800, 450);
            this.ResumeLayout(false);

        }

        #endregion

        private ClosableTabControlHeaderPanel TabHeaderSpace;
        private TabPagePanel TabPagePanel;
    }
}
