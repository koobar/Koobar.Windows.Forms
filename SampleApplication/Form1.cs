using Koobar.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SampleApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
        }

        /// <summary>
        /// デモ表示用のWaveformViewerを作成する。
        /// </summary>
        /// <returns></returns>
        private WaveformViewer CreateDemonstrationWaveformViewer()
        {
            float phase = 0;            // 正弦波の位相
            float freq = 440;           // 正弦波の周波数
            float sampleRate = 44100;   // 正弦波のサンプリング周波数

            var viewer = new WaveformViewer();
            viewer.BackColor = Color.Black;
            viewer.WaveformColor = Color.Green;
            viewer.CenterLineColor = Color.White;
            viewer.DrawCenterLine = true;
            viewer.RealtimeMode = false;
            viewer.UpdateInterval = 20;
            viewer.MaxSamples = 100;
            viewer.UseAntiAlias = true;
            viewer.WaveformLineWidth = 2;

            // 一定間隔でWaveformViewerに正弦波を与えるためのタイマー
            var timer = new Timer();
            timer.Interval = 15;
            timer.Tick += delegate
            {
                phase += freq / sampleRate;
                phase -= (float)Math.Floor(phase);
                viewer.AddSample((float)Math.Sin(2 * Math.PI * phase));
            };

            viewer.Start();
            timer.Start();

            return viewer;
        }

        /// <summary>
        /// デモ表示用のListViewExを作成する。
        /// </summary>
        /// <returns></returns>
        private ListViewEx CreateDemonstrationListViewEx()
        {
            var result = new ListViewEx();
            result.View = View.Details;
            result.DrawItemBorderLines = true;
            result.DrawColumnHeaderBySystem = false;
            result.HeaderStyle = ColumnHeaderStyle.Clickable;
            result.FullRowSelect = true;
            result.ColumnHeaderContextMenuStrip = new ContextMenuStrip()
            {
                Items =
                {
                    "Header Menu1",
                    "Header Menu2"
                }
            };
            result.ItemContextMenuStrip = new ContextMenuStrip()
            {
                Items =
                {
                    "Items Menu1",
                    "Items Menu2"
                }
            };
            result.ColumnHeaderFont = new Font("メイリオ", 15);
            result.SetSubItemAlignment(0, StringAlignment.Far);
            result.SetSubItemAlignment(1, StringAlignment.Center);
            result.SetSubItemAlignment(2, StringAlignment.Near);

            result.Columns.Add("SubItem1");
            result.Columns.Add("SubItem2");
            result.Items.Add("Text");
            result.Items.Add(new ListViewItem { SubItems = { "SubItem1", "SubItem2" }, Text = "Text" });
            result.Items.Add(new ListViewItem { SubItems = { "SubItem1", "SubItem2" }, Text = "Text" });
            result.Items.Add(new ListViewItem { SubItems = { "SubItem1", "SubItem2" }, Text = "Text" });

            return result;
        }

        private Koobar.Windows.Forms.TabPage CreateWaveformViewerTabPage()
        {
            var page = new Koobar.Windows.Forms.TabPage("WaveformViewer");
            page.Control = CreateDemonstrationWaveformViewer();

            return page;
        }

        private Koobar.Windows.Forms.TabPage CreateDoubleBufferedListViewTabPage()
        {
            var page = new Koobar.Windows.Forms.TabPage("ListViewEx");
            page.Control = CreateDemonstrationListViewEx();

            return page;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.closableTabControl1.AddTabPage(CreateWaveformViewerTabPage());
            this.closableTabControl1.AddTabPage(CreateDoubleBufferedListViewTabPage());
        }
    }
}
