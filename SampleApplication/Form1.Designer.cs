namespace SampleApplication
{
    partial class Form1
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.nativeMenuStrip1 = new Koobar.Windows.Forms.NativeMenuStrip();
            this.ファイルFToolStripMenuItem = new Koobar.Windows.Forms.NativeToolStripMenuItem();
            this.新規作成NToolStripMenuItem = new Koobar.Windows.Forms.NativeToolStripMenuItem();
            this.closableTabControl1 = new Koobar.Windows.Forms.ClosableTabControl();
            this.サブメニューToolStripMenuItem = new Koobar.Windows.Forms.NativeToolStripMenuItem();
            this.nativeMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nativeMenuStrip1
            // 
            this.nativeMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem});
            this.nativeMenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.nativeMenuStrip1.Name = "nativeMenuStrip1";
            this.nativeMenuStrip1.Size = new System.Drawing.Size(800, 24);
            this.nativeMenuStrip1.TabIndex = 0;
            this.nativeMenuStrip1.Text = "nativeMenuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新規作成NToolStripMenuItem});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // 新規作成NToolStripMenuItem
            // 
            this.新規作成NToolStripMenuItem.Checked = true;
            this.新規作成NToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.新規作成NToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.サブメニューToolStripMenuItem});
            this.新規作成NToolStripMenuItem.Name = "新規作成NToolStripMenuItem";
            this.新規作成NToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.新規作成NToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.新規作成NToolStripMenuItem.Text = "新規作成(&N)";
            // 
            // closableTabControl1
            // 
            this.closableTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.closableTabControl1.Location = new System.Drawing.Point(0, 24);
            this.closableTabControl1.Name = "closableTabControl1";
            this.closableTabControl1.SelectedIndex = -1;
            this.closableTabControl1.SelectedTab = null;
            this.closableTabControl1.Size = new System.Drawing.Size(800, 426);
            this.closableTabControl1.TabIndex = 1;
            // 
            // サブメニューToolStripMenuItem
            // 
            this.サブメニューToolStripMenuItem.Name = "サブメニューToolStripMenuItem";
            this.サブメニューToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.サブメニューToolStripMenuItem.Text = "サブメニュー";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.closableTabControl1);
            this.Controls.Add(this.nativeMenuStrip1);
            this.MainMenuStrip = this.nativeMenuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.nativeMenuStrip1.ResumeLayout(false);
            this.nativeMenuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Koobar.Windows.Forms.NativeMenuStrip nativeMenuStrip1;
        private Koobar.Windows.Forms.NativeToolStripMenuItem ファイルFToolStripMenuItem;
        private Koobar.Windows.Forms.NativeToolStripMenuItem 新規作成NToolStripMenuItem;
        private Koobar.Windows.Forms.ClosableTabControl closableTabControl1;
        private Koobar.Windows.Forms.NativeToolStripMenuItem サブメニューToolStripMenuItem;
    }
}

